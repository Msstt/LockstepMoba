using System;
using System.Collections.Generic;
using Google.Protobuf;
using Network;

namespace Framework.Network {
    public interface IMsgHandler {
        public void Handle(IMessage msg);
        public void Add(Delegate handler);
        public void Remove(Delegate handler);
    }

    public class MsgHandler<T> : IMsgHandler where T : IMessage {
        public Action<T> handlers = null;
        
        public void Handle(IMessage iMsg) {
            if (iMsg is not T msg) {
                return;
            }
            handlers?.Invoke(msg);
        }
        
        public void Add(Delegate iHandler) {
            if (iHandler is not Action<T> handler) {
                return;
            }
            handlers += handler;
        }
        
        public void Remove(Delegate iHandler) {
            if (iHandler is not Action<T> handler) {
                return;
            }
            handlers -= handler;
        }
    }
    
    public class MsgDispatcher {
        private Dictionary<MessageDef, IMsgHandler> msgHandlers = new Dictionary<MessageDef, IMsgHandler>();
        
        public void RegisterHandler<T>(MessageDef msgId, Action<T> handler) where T : IMessage {
            if (MessageMapping.type[msgId] != typeof(T)) {
                Log.Error("Handler type does not match message type for msgId: {0}", msgId);
                return;
            }

            if (!msgHandlers.ContainsKey(msgId)) {
                msgHandlers.Add(msgId, new MsgHandler<T>());
            }
            msgHandlers[msgId].Add(handler);
        }

        public void UnRegisterHandler<T>(MessageDef msgId, Action<T> handler) where T : IMessage {
            if (MessageMapping.type[msgId] != typeof(T)) {
                Log.Error("Handler type does not match message type for msgId: {0}", msgId);
                return;
            }

            if (!msgHandlers.ContainsKey(msgId)) {
                return;
            }
            msgHandlers[msgId].Remove(handler);
        }
        
        public void Dispatch(Message msg) {
            if (!msgHandlers.ContainsKey(msg.msgId)) {
                Log.Warning("Received message without handler : {0}", msg.msgId);
                return;
            }

            try {
                msgHandlers[msg.msgId].Handle(msg.data);
            } catch (Exception e) {
                Log.Error(e.ToString());
                Log.Error("Exception when handling message {0}: {1}", msg.msgId, e.Message);
            }
        }
    }
}