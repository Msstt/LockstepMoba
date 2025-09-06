using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Framework.Network {
    public class Writer {
        private Socket socket;
        private ConcurrentQueue<Message> queue = new ConcurrentQueue<Message>();

        public void SetSocket(Socket socket) {
            this.socket = socket;
        }
        
        public void Send(Message msg) {
            queue.Enqueue(msg);
        }

        public void Flush() {
            if (socket == null || !socket.Connected) {
                return;
            }
            if (!queue.TryDequeue(out Message msg)) {
                return;
            }

            try {
                socket.Send(BitConverter.GetBytes(msg.data.Length));
                socket.Send(BitConverter.GetBytes((int)msg.msgId));
                socket.Send(msg.data);
            } catch (Exception e) {
                Log.Error("Failed to send message: {0}", e.Message);
                Network.Instance.TryDisconnect();
            }
        }
    }
}