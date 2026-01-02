using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;

namespace Network {
    public static class NetworkDef {
        public static void RegisterDispatcher() {
            TestMsgDispatcher.Register();
            FrameMsgDispatcher.Register();
            BattleMsgDispatcher.Register();
        }

        public static class InputMsgDef {
            public static SortedDictionary<MessageDef, Action<IMessage, battle_input>> setter = new SortedDictionary<MessageDef, Action<IMessage, battle_input>>() {
                { MessageDef.test_input, (msg, inputMsg) => { inputMsg.Test = msg as test_input; } },
                { MessageDef.skill_input, (msg, inputMsg) => { inputMsg.Skill = msg as skill_input; } },
            };
            
            public static SortedDictionary<MessageDef, Func<battle_input, IMessage>> getter = new SortedDictionary<MessageDef, Func<battle_input, IMessage>>() {
                { MessageDef.test_input, (msg) => msg.Test },
                { MessageDef.skill_input, (msg) => msg.Skill },
            };
            
            public static SortedDictionary<MessageDef, Func<IDictionary>> creator = new SortedDictionary<MessageDef, Func<IDictionary>>() {
                { MessageDef.test_input, () => new SortedDictionary<Uid, test_input>() },
                { MessageDef.skill_input, () => new SortedDictionary<Uid, skill_input>() },
            };
        }
    }
}
