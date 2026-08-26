using System.Collections.Concurrent;
using Google.Protobuf;
using Network;

namespace Framework.Network {
    public interface IMsgHandler {
        public void Handle(Uid uid, IMessage iMsg);
        public void Add(Delegate handler);
        public void Remove(Delegate handler);
    }

    public class MsgHandler<T> : IMsgHandler where T : IMessage {
        private Action<Uid, T> handlers = null;
        
        public void Handle(Uid uid, IMessage iMsg) {
            if (iMsg is not T msg) {
                return;
            }
            handlers?.Invoke(uid, msg);
        }
        
        public void Add(Delegate iHandler) {
            if (iHandler is not Action<Uid, T> handler) {
                return;
            }
            handlers += handler;
        }
        
        public void Remove(Delegate iHandler) {
            if (iHandler is not Action<Uid, T> handler) {
                return;
            }
            handlers -= handler;
        }
    }
    
    public class Network : Singleton<Network> {
        const int DispatchMsgFrameCount = 10;
        
        private int port;
        
        private Listener listener;
        
        private ConcurrentDictionary<Uid, Client> clients = new ConcurrentDictionary<Uid, Client>();
        
        private ConcurrentQueue<Message> msgQueue = new ConcurrentQueue<Message>();
        
        private Dictionary<MessageDef, IMsgHandler> msgHandlers = new Dictionary<MessageDef, IMsgHandler>();

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
                Uid uid = GetUid();
                if (uid > Config.Instance.Network.auto_start_count) {
                    tcpClient.Close();
                } else {
                    Client client = new Client(uid, tcpClient);
                    clients.TryAdd(client.Uid, client);
                    EventMgr.Instance.Send(new EventType.OnPlayerConnected {
                        uid = client.Uid,
                    });
                    
                    ThreadPool.QueueUserWorkItem(_ => clients[client.Uid].FlushRead());
                    ThreadPool.QueueUserWorkItem(_ => clients[client.Uid].FlushWrite());
                }
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
                
                if (!msgHandlers.ContainsKey(msg.msgId)) {
                    Log.Warning("Received message without handler: {0}", msg.msgId);
                    continue;
                }

                try {
                    msgHandlers[msg.msgId].Handle(msg.client, msg.data);
                } catch (Exception e) {
                    Log.Error(e.ToString());
                    Log.Error("Exception when handling message {0}: {1}", msg.msgId, e.Message);
                }
            }
        }
        
        public void RegisterMsgHandler<T>(MessageDef msgId, Action<Uid, T> handler) where T : IMessage {
            if (MessageMapping.type[msgId] != typeof(T)) {
                Log.Error("Handler type does not match message type for msgId: {0}", msgId);
                return;
            }

            if (!msgHandlers.ContainsKey(msgId)) {
                msgHandlers.Add(msgId, new MsgHandler<T>());
            }
            msgHandlers[msgId].Add(handler);
        }
        
        public void RemoveMsgHandler<T>(MessageDef msgId, Action<Uid, T> handler) where T : IMessage { 
            if (MessageMapping.type[msgId] != typeof(T)) {
                Log.Error("Handler type does not match message type for msgId: {0}", msgId);
                return;
            }

            if (!msgHandlers.ContainsKey(msgId)) {
                return;
            }
            msgHandlers[msgId].Remove(handler);
        }

        #endregion
    }
}
