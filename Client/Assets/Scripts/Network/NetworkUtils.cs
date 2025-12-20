using System;
using System.Collections.Generic;
using Framework;
using Framework.Network;
using Google.Protobuf;
using UnityEngine;

namespace Network {
    public static class NetworkUtils {
        
        public static void Send(MessageDef msgId, IMessage msg) {
            GameMgr.Instance.GetSystem<INetwork>().Send(msgId, msg);
        }

        public static void RegisterCollector<T>(MessageDef id, Func<T> collector) where T : IMessage, new() {
            GameMgr.Instance.GetSystem<ILockStep>().RegisterCollector(id, collector);
        }
        public static void RemoveCollector<T>(MessageDef id, Func<T> collector) where T : IMessage, new() {
            GameMgr.Instance.GetSystem<ILockStep>().RemoveCollector(id, collector);
        }
        public static void RegisterHandler<T>(MessageDef id, Action<SortedDictionary<Uid, T>> handler) where T : IMessage {
            GameMgr.Instance.GetSystem<ILockStep>().RegisterHandler(id, handler);
            
        }
        public static void RemoveHandler<T>(MessageDef id, Action<SortedDictionary<Uid, T>> handler) where T : IMessage {
            GameMgr.Instance.GetSystem<ILockStep>().RemoveHandler(id, handler);
            
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
