global using Uid = int;

using System.Net.Sockets;
using Google.Protobuf;
using Network;

namespace Framework.Network {
    public class Client {
        private static readonly int MaxBufferSize = 4096;
        public Uid Uid { get; private set; }
        
        private TcpClient socket;
        public TcpClient Socket { get => socket; }

        private int writeBufferIndex = 0;
        private byte[] writeBuffer = new byte[MaxBufferSize];
        
        private int readBufferIndex = 0;
        private byte[] readBuffer = new byte[MaxBufferSize];
        
        private object lockObject = new object();
        private bool isDisconnected = false;

        public Client(int uid, TcpClient socket) {
            Uid = uid;
            this.socket = socket;
        }

        public void Send(MessageDef msgId, IMessage data) {
            byte[] buffer = data.ToByteArray();
            Send(BitConverter.GetBytes(buffer.Length));
            Send(BitConverter.GetBytes((int)msgId));
            Send(buffer);
        }
        
        private void Send(byte[] data) {
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
            IMessage data = null;
            try {
                data = MessageParserDef.Parsers[(MessageDef)msgId].ParseFrom(readBuffer, 8, msgLen);
            } catch (Exception e) {
                Log.Error("Failed to parse message {0}: {1}", msgId, e.Message);
            }
            Network.Instance.PushMsg(Uid, (MessageDef)msgId, data);
            Array.Copy(readBuffer, 8 + msgLen, readBuffer, 0, readBufferIndex - (8 + msgLen));
            readBufferIndex -= 8 + msgLen;
            return true;
        }

        public void FlushRead() {
            lock (lockObject) {
                if (isDisconnected) {
                    return;
                }
            }
            if (!socket.Connected) {
                return;
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
            if (!socket.Connected) {
                return;
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
            if (!socket.Connected) {
                return;
            }
            Log.Info("Disconnect tcp client: {0}", socket.Client.RemoteEndPoint);
            socket.Close();
            Network.Instance.OnDisconnect(this);
        }
    }
}
