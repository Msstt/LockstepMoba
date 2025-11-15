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

        public static void SetInputMsgField(ref frame_input_c2s msg, Func<MessageDef, IMessage> collectFunc) {
            if (collectFunc(MessageDef.test_input) is test_input test_input) msg.Input.Test = test_input;
        }
        
        public static void SetInputMsgField(frame_input_s2c msg, ref Dictionary<MessageDef, IDictionary> inputs) {
            void SetField<T>(ref Dictionary<MessageDef, IDictionary> inputs, MessageDef id, Func<battle_input, IMessage> getter) where T : class, IMessage{
                Dictionary<Uid, T> input = new Dictionary<Uid, T>();
                foreach (var inputInfo in msg.Inputs) {
                    var msg = getter(inputInfo.Input) as T;
                    if (msg != null) {
                        input[inputInfo.Uid] = msg;
                    }
                }
                inputs[id] = input;
            }

            SetField<test_input>(ref inputs, MessageDef.test_input, (msg) => msg.Test);
        }
    }
}
