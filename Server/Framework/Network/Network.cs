using System.Collections.Concurrent;
using System.Net.Sockets;
using Google.Protobuf;
using Network;

namespace Framework.Network {
    public class Network : Singleton<Network> {
        const int DispatchMsgFrameCount = 10;
        
        private int port;
        
        private Listener listener;
        
        private ConcurrentDictionary<Uid, Client> clients = new ConcurrentDictionary<Uid, Client>();
        
        private ConcurrentQueue<Message> msgQueue = new ConcurrentQueue<Message>();
        
        private Dictionary<MessageDef, Delegate> msgHandlers = new Dictionary<MessageDef, Delegate>();

        public Network() {
            foreach (MessageDef value in Enum.GetValues(typeof(MessageDef))) {
                msgHandlers.Add(value, null);
            }
        }

        private int GetUid() {
            int uid = 1;
            while (clients.ContainsKey(uid)) {
                uid++;
            }
            return uid;
        }
        
        private void AcceptClient() {
            var tcpClient = listener.Accept();
            if (tcpClient != null) {
                Client client = new Client(GetUid(), tcpClient);
                clients.TryAdd(client.Uid, client);
                EventMgr.Instance.Send(EventDef.OnPlayerConnected, client.Uid);
                
                ThreadPool.QueueUserWorkItem(_ => clients[client.Uid].FlushRead());
                ThreadPool.QueueUserWorkItem(_ => clients[client.Uid].FlushWrite());
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
            clients.TryRemove(client.Uid, out _);
        }
        
        public void PushMsg(Uid uid, MessageDef msgId, IMessage data) {
            msgQueue.Enqueue(new Message() {
                client = uid,
                msgId = msgId,
                data = data,
            });
        }
        
        public void Send(Uid receiver, MessageDef msgId, IMessage msg) {
            if (!clients.TryGetValue(receiver, out Client client) || msg == null) {
                return;
            }
            client.Send(msgId, msg);
            FileLog.Instance.Log("Send Message " + msgId + " To: " + receiver + "\n" + msg);
        }
        
        public void Broadcast(MessageDef msgId, IMessage msg) {
            if (msg == null) {
                return;
            }
            foreach (var client in clients.Values) {
                client.Send(msgId, msg);
            }
            FileLog.Instance.Log("Broadcast Message " + msgId + "\n" + msg);
        }

        public void Broadcast(MessageDef msgId, Func<Uid, IMessage> getMsgFunc) {
            foreach (var (uid, client) in clients) {
                var msg = getMsgFunc?.Invoke(uid);
                if (msg != null) {
                    client.Send(msgId, msg);
                    FileLog.Instance.Log("Send Message " + msgId + " To: " + uid + "\n" + msg);
                }
            }
        }
        
        public List<Uid> GetAllClientUid() {
            List<Uid> uids = new List<Uid>();
            foreach (var uid in clients.Keys) {
                uids.Add(uid);
            }
            return uids;
        }

        #region 消息分发

        public void DispatchMsg() {
            for (int i = 0; i < DispatchMsgFrameCount; i++) {
                if (!msgQueue.TryDequeue(out Message msg)) {
                    break;
                }
                FileLog.Instance.Log("Receive Message " + msg.msgId + " from: " + msg.client + "\n" + msg.data);
                msgHandlers[msg.msgId]?.DynamicInvoke(msg.client, msg.data);
            }
        }
        
        public void RegisterMsgHandler<T>(MessageDef msgDef, Action<Uid, T> handler) where T : IMessage {
            msgHandlers[msgDef] = Delegate.Combine(msgHandlers[msgDef], handler);
        }
        
        public void RemoveMsgHandler<T>(MessageDef msgDef, Action<Uid, T> handler) where T : IMessage { 
            msgHandlers[msgDef] = Delegate.Remove(msgHandlers[msgDef], handler);
        }

        #endregion
    }
}
