using System;
using System.Collections.Generic;
using Battle;
using Framework;
using Framework.Network;
using Google.Protobuf;

namespace Network {
    public class LockStep : Singleton<LockStep> {
        #region 输入消息映射
        
        private static readonly Dictionary<string, MessageDef> String2Id = new Dictionary<string, MessageDef>() {
            { "test", MessageDef.test_input },
        };
        
        #endregion
        
        public int Frame { get; private set; }
        
        private Dictionary<int, frame_input_s2c> allInputs = new Dictionary<int, frame_input_s2c>();
        
        public delegate void InputHandler(Dictionary<Uid, IMessage> inputs);
        private Dictionary<MessageDef, InputHandler> inputHandlers = new Dictionary<MessageDef, InputHandler>();
        
        public delegate IMessage InputCollector();
        private Dictionary<MessageDef, InputCollector> inputCollectors = new Dictionary<MessageDef, InputCollector>();

        private void Clear() {
            Frame = 0;
            allInputs = new Dictionary<int, frame_input_s2c>();
            inputHandlers = new Dictionary<MessageDef, InputHandler>();
            inputCollectors = new Dictionary<MessageDef, InputCollector>();
        }

        public void Start() {
            Clear();
        }

        #region 发送

        public void RegisterCollector(MessageDef id, InputCollector collector) {
            if (!inputCollectors.ContainsKey(id)) {
                inputCollectors.Add(id, collector);
            } else {
                inputCollectors[id] += collector;
            }
        }
        
        public void RemoveCollector(MessageDef id, InputCollector collector) {
            if (inputCollectors.ContainsKey(id)) {
                inputCollectors[id] -= collector;
            }
        }

        public frame_input_c2s GetInputMsg() {
            frame_input_c2s msg = new frame_input_c2s() {
                Frame = Frame + 1,
                Input = new battle_input(),
            };
            foreach (var field in battle_input.Descriptor.Fields.InFieldNumberOrder()) {
                MessageDef id = String2Id[field.Name];
                IMessage input = Collect(id);
                if (input != null) {
                    field.Accessor.SetValue(msg.Input, input);
                }
            }
            return msg;
        }
        
        private IMessage Collect(MessageDef id) {
            if (!inputCollectors.ContainsKey(id)) {
                return null;
            }
            
            try {
                return inputCollectors[id]?.Invoke();
            } catch (Exception e) {
                Log.Error(e.ToString());
                Log.Error("Exception when collecting input msg {0}: {1}", id, e.Message);
                return null;
            }
        }

        #endregion

        #region 接收
        
        public void RegisterHandler(MessageDef id, InputHandler handler) {
            if (!inputHandlers.ContainsKey(id)) {
                inputHandlers.Add(id, handler);
            } else {
                inputHandlers[id] += handler;
            }
        }
        
        public void RemoveHandler(MessageDef id, InputHandler handler) {
            if (inputHandlers.ContainsKey(id)) {
                inputHandlers[id] -= handler;
            }
        }

        public void PushInputMsg(frame_input_s2c msg) {
            if (allInputs.ContainsKey(msg.Frame)) {
                Log.Error("Duplicate frame input from server: " + msg.Frame);
                return;
            }
            allInputs.Add(msg.Frame, msg);
            UpdateFrame();
        }

        private void UpdateFrame() {
            // TODO 当收到不连续帧时，加速表现层，而不是一帧内直接更新
            while (allInputs.ContainsKey(Frame + 1)) {
                Frame++;
                HandleFrameInputs(allInputs[Frame]);
            }
        }
        
        private void HandleFrameInputs(frame_input_s2c frameInput) {
            foreach (var field in battle_input.Descriptor.Fields.InFieldNumberOrder()) {
                MessageDef id = String2Id[field.Name];
                Dictionary<Uid, IMessage> inputs = new Dictionary<Uid, IMessage>();
                foreach (var input in frameInput.Inputs) {
                    inputs[input.Uid] = field.Accessor.GetValue(input.Input) as IMessage;
                }
                Dispatch(id, inputs);
                
            }
        }
        
        private void Dispatch(MessageDef id, Dictionary<Uid, IMessage> inputs) {
            if (!inputHandlers.ContainsKey(id)) {
                return;
            }

            try {
                inputHandlers[id]?.Invoke(inputs);
            } catch (Exception e) {
                Log.Error(e.ToString());
                Log.Error("Exception when handling input msg {0}: {1}", id, e.Message);
            }
        }

        #endregion
    }
}