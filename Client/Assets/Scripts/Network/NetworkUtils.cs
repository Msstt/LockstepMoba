using System.Reflection;
using Framework;
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
                    Framework.Network.MsgDispatcher.Instance.RegisterHandler(attr.Id, (msg) => {
                        method.Invoke(null, new object[] { msg });
                    });
                }
            }
            
            Framework.Network.Network.Instance.Connect("127.0.0.1", 9980);
            
            Updater.Instance.RegisterUpdate(NetworkUtils.Update);
        }
        
        // 网络层更新
        private static void Update() {
            Framework.Network.Network.Instance.DispatchMsg();
        }
        
        public static void Send(MessageDef msgId, IMessage msg) {
            Framework.Network.Network.Instance.Send(msgId, msg);
        }
    }
}
