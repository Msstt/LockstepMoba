using System;
using System.Collections;
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
        
        public delegate void InputHandler<T>(Dictionary<Uid, T> inputs);
        private Dictionary<MessageDef, Delegate> inputHandlers = new Dictionary<MessageDef, Delegate>();
        
        public delegate T InputCollector<T>();
        private Dictionary<MessageDef, Delegate> inputCollectors = new Dictionary<MessageDef, Delegate>();

        private void Clear() {
            Frame = 0;
            allInputs = new Dictionary<int, frame_input_s2c>();
            inputHandlers = new Dictionary<MessageDef, Delegate>();
            inputCollectors = new Dictionary<MessageDef, Delegate>();

            EventUtils.Remove(EventDef.OnConnected, ReqFrameData);
        }

        public void Start() {
            Clear();

            EventUtils.Register(EventDef.OnConnected, ReqFrameData);
            ReqFrameData();
            
            EventUtils.Send(EventDef.OnLockStepStart);
        }

        public void ReqFrameData() {
            NetworkUtils.Send(MessageDef.frame_reconnect_c2s, new frame_reconnect_c2s {
                Frame = Frame + 1,
            });
        }

        #region 发送

        public void RegisterCollector<T>(MessageDef id, InputCollector<T> collector) {
            if (!inputCollectors.ContainsKey(id)) {
                inputCollectors.Add(id, collector);
            } else {
                inputCollectors[id] = Delegate.Combine(inputCollectors[id], collector);
            }
        }
        
        public void RemoveCollector<T>(MessageDef id, InputCollector<T> collector) {
            if (inputCollectors.ContainsKey(id)) {
                inputCollectors[id] = Delegate.Remove(inputCollectors[id], collector);
            }
        }

        public frame_input_c2s GetInputMsg() {
            frame_input_c2s msg = new frame_input_c2s() {
                Frame = Frame + 1,
                Input = new battle_input(),
            };
            NetworkDef.SetInputMsgField(ref msg, Collect);
            return msg;
        }
        
        private IMessage Collect(MessageDef id) {
            if (!inputCollectors.ContainsKey(id)) {
                return null;
            }
            
            try {
                return inputCollectors[id]?.DynamicInvoke() as IMessage;
            } catch (Exception e) {
                Log.Error(e.ToString());
                Log.Error("Exception when collecting input msg {0}: {1}", id, e.Message);
                return null;
            }
        }

        #endregion

        #region 接收
        
        public void RegisterHandler<T>(MessageDef id, InputHandler<T> handler) {
            if (!inputHandlers.ContainsKey(id)) {
                inputHandlers.Add(id, handler);
            } else {
                inputHandlers[id] = Delegate.Combine(inputHandlers[id], handler);
            }
        }
        
        public void RemoveHandler<T>(MessageDef id, InputHandler<T> handler) {
            if (inputHandlers.ContainsKey(id)) {
                inputHandlers[id] = Delegate.Remove(inputHandlers[id], handler);
            }
        }

        public void PushInputMsg(frame_input_s2c msg) {
            if (allInputs.ContainsKey(msg.Frame)) {
                Log.Warning("Duplicate frame input from server: " + msg.Frame);
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
            Dictionary<MessageDef, IDictionary> inputs = new Dictionary<MessageDef, IDictionary>();
            NetworkDef.SetInputMsgField(frameInput, ref inputs);
            foreach (var (id, input) in inputs) {
                Dispatch(id, input);
            }
        }
        
        private void Dispatch(MessageDef id, IDictionary inputs) {
            if (!inputHandlers.ContainsKey(id)) {
                return;
            }

            try {
                inputHandlers[id]?.DynamicInvoke(inputs);
            } catch (Exception e) {
                Log.Error(e.ToString());
                Log.Error("Exception when handling input msg {0}: {1}", id, e.Message);
            }
        }

        #endregion
    }
}