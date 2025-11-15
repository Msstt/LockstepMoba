using System;
using System.Collections.Generic;
using Google.Protobuf;
using Network;

namespace Framework.Network {
    public class MsgDispatcher : Singleton<MsgDispatcher> {
        private Dictionary<MessageDef, Delegate> msgHandlers = new Dictionary<MessageDef, Delegate>();

        public MsgDispatcher() {
            foreach (MessageDef value in Enum.GetValues(typeof(MessageDef))) {
                msgHandlers.Add(value, null);
            }
        }
        
        public void RegisterHandler<T>(MessageDef msgId, Action<T> handler) where T : IMessage {
            msgHandlers[msgId] = Delegate.Combine(msgHandlers[msgId], handler);
        }

        public void RemoveHandler(MessageDef msgId, Action<IMessage> handler) {
            msgHandlers[msgId] = Delegate.Remove(msgHandlers[msgId], handler);
        }
        
        public void Dispatch(Message msg) {
            if (!msgHandlers.ContainsKey(msg.msgId)) {
                Log.Warning("Received message with invalid msgId : {0}", msg.msgId);
                return;
            }

            try {
                msgHandlers[msg.msgId]?.DynamicInvoke(msg.data);
            } catch (Exception e) {
                Log.Error(e.ToString());
                Log.Error("Exception when handling message {0}: {1}", msg.msgId, e.Message);
            }
        }
    }
}