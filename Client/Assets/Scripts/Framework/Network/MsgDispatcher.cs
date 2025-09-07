using System;
using System.Collections.Generic;
using Network;

namespace Framework.Network {
    public class MsgDispatcher : Singleton<MsgDispatcher> {
        private Dictionary<MessageDef, Action<Message>> msgHandlers = new Dictionary<MessageDef, Action<Message>>();

        public MsgDispatcher() {
            for (int i = 1; i <= Enum.GetValues(typeof(MessageDef)).Length; i++) {
                msgHandlers.TryAdd((MessageDef)i, null);
            }
        }
        
        public void RegisterHandler(MessageDef msgId, Action<Message> handler) {
            msgHandlers[msgId] += handler;
        }

        public void RemoveHandler(MessageDef msgId, Action<Message> handler) {
            msgHandlers[msgId] -= handler;
        }
        
        public void Dispatch(Message msg) {
            if (!msgHandlers.ContainsKey(msg.msgId)) {
                Log.Warning("Received message with invalid msgId : {0}", msg.msgId);
                return;
            }

            try {
                msgHandlers[msg.msgId]?.Invoke(msg);
            } catch (Exception e) {
                Log.Error(e.ToString());
                Log.Error("Exception when handling message {0}: {1}", msg.msgId, e.Message);
            }
        }
    }
}