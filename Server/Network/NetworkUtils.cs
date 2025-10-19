using System.Reflection;
using Google.Protobuf;

namespace Network {
    public static class NetworkUtils {
        // 网络层初始化
        public static void Start() {
            // 注册 Dispatcher
            foreach (var dispatcher in NetworkDef.Dispatcher) {
                MethodInfo[] methods = dispatcher.GetMethods(BindingFlags.Public | BindingFlags.Static);
                foreach (var method in methods) {
                    var attr = method.GetCustomAttribute<MessageAttribute>();
                    if (attr == null) {
                        continue;
                    }
                    Framework.Network.Network.Instance.RegisterMsgHandler(attr.Id, (client, msg) => {
                        method.Invoke(null, new object[] { client, msg });
                    });
                }
            }
            
            Framework.Network.Network.Instance.Start(Config.Instance.Network.port);
        }
        
        // 网络层更新
        public static void Update() {
            Framework.Network.Network.Instance.DispatchMsg();
            LockStep.Instance.Update();
        }

        public static void Send(Uid receiver, MessageDef msgId, IMessage msg) {
            Framework.Network.Network.Instance.Send(receiver, msgId, msg);
        }
        
        public static void Broadcast(MessageDef msgId, IMessage msg) {
            Framework.Network.Network.Instance.Broadcast(msgId, msg);
        }

        public static void Broadcast(MessageDef msgId, Func<Uid, IMessage> getMsgFunc) {
            Framework.Network.Network.Instance.Broadcast(msgId, getMsgFunc);
        }
        
        public static List<Uid> GetAllClientUid() {
            return Framework.Network.Network.Instance.GetAllClientUid();
        }
    }
}
