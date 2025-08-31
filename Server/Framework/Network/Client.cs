using System.Net.Sockets;

namespace Framework.Network {
    public class Client {
        private static readonly int MaxBufferSize = 4096;
        
        private TcpClient socket;
        public TcpClient Socket { get => socket; }

        private int writeBufferIndex = 0;
        private byte[] writeBuffer = new byte[MaxBufferSize];
        
        private int readBufferIndex = 0;
        private byte[] readBuffer = new byte[MaxBufferSize];
        
        private object lockObject = new object();
        private bool isDisconnected = false;

        public Client(TcpClient socket) {
            this.socket = socket;
        }

        public void Send(byte[] data) {
            lock (writeBuffer) {
                if (writeBufferIndex + data.Length > MaxBufferSize) {
                    Log.Error("Write buffer overflow");
                    return;
                }
                for (int i = writeBufferIndex; i < writeBufferIndex + data.Length; i++) {
                    writeBuffer[i] = data[i - writeBufferIndex];
                }
                writeBufferIndex += data.Length;
            }
        }
        
        private bool ParseMsg() {
            if (readBufferIndex < 8) {
                return false;
            }
            int msgLen = BitConverter.ToInt32(readBuffer, 0);
            if (readBufferIndex < msgLen + 8) {
                return false;
            }
            int msgId = BitConverter.ToInt32(readBuffer, 4);
            byte[] msgData = new byte[msgLen];
            Array.Copy(readBuffer, 8, msgData, 0, msgLen);
            Network.Instance.PushMsg(this, msgId, msgData);
            Array.Copy(readBuffer, 8 + msgLen, readBuffer, 0, readBufferIndex - (8 + msgLen));
            return true;
        }

        public void FlushRead() {
            lock (lockObject) {
                if (isDisconnected) {
                    return;
                }
            }
            try {
                var stream = socket.GetStream();
                int byteCount = stream.Read(readBuffer, readBufferIndex, readBuffer.Length - readBufferIndex);
                if (byteCount == 0) {
                    Disconnect();
                    return;
                }
                readBufferIndex += byteCount;
            } catch (Exception ex) {
                Disconnect();
                Log.Error("Read error: {0}", ex.Message);
            }
            while (ParseMsg());
            ThreadPool.QueueUserWorkItem(_ => FlushRead());
        }
        
        public void FlushWrite() {
            lock (lockObject) {
                if (isDisconnected) {
                    return;
                }
            }
            try {
                lock (writeBuffer) {
                    var stream = socket.GetStream();
                    stream.Write(writeBuffer, 0, writeBufferIndex);
                    writeBufferIndex = 0;
                }
            } catch (Exception ex) {
                Disconnect();
                Log.Error("Write error: {0}", ex.Message);
            }
            ThreadPool.QueueUserWorkItem(_ => FlushWrite());
        }

        public void Disconnect() {
            lock (lockObject) {
                isDisconnected = true;
            }
            Log.Info("Disconnect tcp client: {0}", socket.Client.RemoteEndPoint);
            socket.Close();
            Network.Instance.OnDisconnect(this);
        }
    }
}
