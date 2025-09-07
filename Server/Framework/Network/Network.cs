using System.Collections.Concurrent;
using System.Net.Sockets;
using Google.Protobuf;
using Network;

namespace Framework.Network {
    public class Network : Singleton<Network> {
        const int DispatchMsgFrameCount = 10;
        
        private int port;
        
        private Listener listener;
        
        private ConcurrentDictionary<TcpClient, Client> clients = new ConcurrentDictionary<TcpClient, Client>();
        
        private ConcurrentQueue<Message> msgQueue = new ConcurrentQueue<Message>();
        
        private Dictionary<MessageDef, Action<Message>> msgHandlers = new Dictionary<MessageDef, Action<Message>>();

        public Network() {
            for (int i = 1; i <= Enum.GetValues(typeof(MessageDef)).Length; i++) {
                msgHandlers.Add((MessageDef)i, null);
            }
        }
        
        private void AcceptClient() {
            var client = listener.Accept();
            if (client != null) {
                clients.TryAdd(client, new Client(client));
                ThreadPool.QueueUserWorkItem(_ => clients[client].FlushRead());
                ThreadPool.QueueUserWorkItem(_ => clients[client].FlushWrite());
            }
            ThreadPool.QueueUserWorkItem(_ => AcceptClient());
        }

        public void Start(int port) {
            this.port = port;
            listener = new Listener();
            if (!listener.Listen(port)) {
                return;
            }
            ThreadPool.QueueUserWorkItem(_ => AcceptClient());
        }

        public void OnDisconnect(Client client) {
            clients.TryRemove(client.Socket, out _);
        }
        
        public void PushMsg(Client client, MessageDef msgId, IMessage data) {
            msgQueue.Enqueue(new Message() {
                client = client,
                msgId = msgId,
                data = data,
            });
        }

        public void Send(Message msg) {
            if (!clients.TryGetValue(msg.client.Socket, out Client client)) {
                return;
            }
            client.Send(msg);
        }

        #region 消息分发

        public void DispatchMsg() {
            for (int i = 0; i < DispatchMsgFrameCount; i++) {
                if (!msgQueue.TryDequeue(out Message msg)) {
                    break;
                }
                Console.WriteLine("Receive Message " + msg);
                msgHandlers[msg.msgId]?.Invoke(msg);
            }
        }
        
        public void RegisterMsgHandler(MessageDef msgDef, Action<Message> handler) {
            msgHandlers[msgDef] += handler;
        }
        
        public void RemoveMsgHandler(MessageDef msgDef, Action<Message> handler) { 
            msgHandlers[msgDef] -= handler;
        }

        #endregion
    }
}
