using System.Reflection;

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
            
            Framework.Network.Network.Instance.Start(9980);
        }
        
        // 网络层更新
        public static void Update() {
            Framework.Network.Network.Instance.DispatchMsg();
        }
    }
}
