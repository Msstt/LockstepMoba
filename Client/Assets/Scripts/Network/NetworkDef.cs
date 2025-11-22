using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Google.Protobuf;

namespace Network {
    public static class NetworkDef {
        public static void RegisterDispatcher() {
            TestMsgDispatcher.Register();
            FrameMsgDispatcher.Register();
            BattleMsgDispatcher.Register();
        }

        public static class InputMsgDef {
            public static Dictionary<MessageDef, Action<IMessage, battle_input>> setter = new Dictionary<MessageDef, Action<IMessage, battle_input>>() {
                { MessageDef.test_input, (msg, inputMsg) => { inputMsg.Test = msg as test_input; } },
            };
            
            public static Dictionary<MessageDef, Func<battle_input, IMessage>> getter = new Dictionary<MessageDef, Func<battle_input, IMessage>>() {
                { MessageDef.test_input, (msg) => { return msg.Test; } },
            };
            
            public static Dictionary<MessageDef, Func<IDictionary>> creator = new Dictionary<MessageDef, Func<IDictionary>>() {
                { MessageDef.test_input, () => { return new Dictionary<Uid, test_input>(); } },
            };
        }
    }
}
