using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Google.Protobuf;

namespace Framework.Network {
    public class Writer {
        private Network system;
        private Socket socket;
        private ConcurrentQueue<Message> queue = new ConcurrentQueue<Message>();

        public Writer(Network system) {
            this.system = system;
        }

        public void SetSocket(Socket socket) {
            this.socket = socket;
        }
        
        public void Send(Message msg) {
            if (queue.Count >= SendConfig.MaxQueueCount) {
                Log.Warning("Message queue full, dropping message {0}", msg.msgId);
                return;
            }
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
                byte[] buffer = msg.data.ToByteArray();
                socket.Send(BitConverter.GetBytes(buffer.Length));
                socket.Send(BitConverter.GetBytes((int)msg.msgId));
                socket.Send(buffer);
            } catch (Exception e) {
                if (e.Message != "interrupted") {
                    Log.Error("Failed to send message: {0}", e.Message);
                    system.TryDisconnect();
                }
            }
        }
    }
}