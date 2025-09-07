using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Network;

namespace Framework.Network {
    public class Reader {
        private Socket socket;
        private ConcurrentQueue<Message> queue = new ConcurrentQueue<Message>();

        private int readIndex = 0;
        private byte[] buffer = new byte[ReceiveConfig.BufferSize];

        public void SetSocket(Socket socket) {
            this.socket = socket;
        }

        public Message? GetMessage() {
            return queue.TryDequeue(out Message msg) ? msg : null;
        }

        public void Flush() {
            if (socket == null || !socket.Connected) {
                return;
            }
            try {
                int byteCount = socket.Receive(buffer, readIndex, ReceiveConfig.BufferSize - readIndex, SocketFlags.None);
                readIndex += byteCount;
                while (ParseMsg()) ;
            } catch (Exception e) {
                if (e.Message != "interrupted") {
                    Log.Error("Failed to receive message: {0}", e.Message);
                    Network.Instance.TryDisconnect();
                }
            }
        }
        
        private bool ParseMsg() {
            if (readIndex < 8) {
                return false;
            }
            int msgLen = BitConverter.ToInt32(buffer, 0);
            if (readIndex < msgLen + 8) {
                return false;
            }
            Message msg = new Message();
            msg.msgId = (MessageDef)BitConverter.ToInt32(buffer, 4);
            try {
                msg.data = MessageParserDef.Parsers[msg.msgId].ParseFrom(buffer, 8, msgLen);
            } catch (Exception e) {
                Log.Error("Failed to parse message {0}: {1}", msg.msgId, e.Message);
            }
            Array.Copy(buffer, 8 + msgLen, buffer, 0, readIndex - (8 + msgLen));
            readIndex -= 8 + msgLen;
            queue.Enqueue(msg);
            return true;
        }
    }
}