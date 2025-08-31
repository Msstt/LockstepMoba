using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Framework.Network {
    public class Network : Singleton<Network> {
        private int port;
        
        private Listener listener;
        
        private ConcurrentDictionary<TcpClient, Client> clients = new ConcurrentDictionary<TcpClient, Client>();
        
        private ConcurrentQueue<Message> msgQueue = new ConcurrentQueue<Message>();
        
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
        
        public void PushMsg(Client client, int msgId, byte[] data) {
            msgQueue.Enqueue(new Message() {
                client = client,
                msgId = msgId,
                data = data
            });
        }
    }
}
