using System;
using Framework;
using Google.Protobuf;
using UnityEngine;

namespace Network {
    public static class NetworkUtils {
        // 网络层初始化
        public static void Start() {
            // 注册 Dispatcher
            NetworkDef.RegisterDispatcher();
            
            Framework.Network.Network.Instance.Connect("127.0.0.1", 9980);
            
            Updater.Instance.RegisterUpdate(NetworkUtils.Update);
            
            NetworkUtils.Send(MessageDef.frame_reconnect_c2s, new frame_reconnect_c2s {
                Frame = 1,
            });
        }
        
        // 网络层更新
        private static void Update() {
            Framework.Network.Network.Instance.DispatchMsg();
        }
        
        public static void Send(MessageDef msgId, IMessage msg) {
            Framework.Network.Network.Instance.Send(msgId, msg);
        }
        
        public static bool CheckMessageType(MessageDef msgId, Type type) {
            if (MessageMapping.type[msgId] != type) {
                Debug.LogError($"Message type does not match for msgId: {msgId}");
                return false;
            }
            return true;
        }
    }
}
