using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Network;
using Google.Protobuf;

namespace Network {
    public class LockStep : ILockStep {
        public int Frame { get; private set; }
        
        private Dictionary<int, frame_input_s2c> allInputs = new Dictionary<int, frame_input_s2c>();
        
        private Dictionary<MessageDef, IInputHandler> inputHandlers = new Dictionary<MessageDef, IInputHandler>();
        private Dictionary<MessageDef, IInputCollector> inputCollectors = new Dictionary<MessageDef, IInputCollector>();

        private void Clear() {
            Frame = 0;
            allInputs = new Dictionary<int, frame_input_s2c>();
            inputHandlers = new Dictionary<MessageDef, IInputHandler>();
            inputCollectors = new Dictionary<MessageDef, IInputCollector>();

            EventUtils.UnRegister<EventType.OnConnected>(ReqFrameData);
        }

        public void Start() {
            Clear();

            EventUtils.Register<EventType.OnConnected>(ReqFrameData);
            ReqFrameData();
            
            EventUtils.Send<EventType.OnLockStepStart>();
        }

        public void ReqFrameData() {
            NetworkUtils.Send(MessageDef.frame_reconnect_c2s, new frame_reconnect_c2s {
                Frame = Frame + 1,
            });
        }

        #region 发送

        public void RegisterCollector<T>(MessageDef id, Func<T> collector) where T : IMessage, new() {
            if (!NetworkUtils.CheckMessageType(id, typeof(T))) {
                return;
            }
            
            if (!inputCollectors.ContainsKey(id)) {
                inputCollectors.Add(id, new InputCollector<T>());
            }
            inputCollectors[id].Add(collector);
        }
        
        public void RemoveCollector<T>(MessageDef id, Func<T> collector) where T : IMessage, new() {
            if (!NetworkUtils.CheckMessageType(id, typeof(T))) {
                return;
            }
            
            if (!inputCollectors.ContainsKey(id)) {
                return;
            }
            inputCollectors[id].Remove(collector);
        }

        public frame_input_c2s GetInputMsg() {
            frame_input_c2s msg = new frame_input_c2s() {
                Frame = Frame + 1,
                Input = new battle_input(),
            };
            foreach (var (msgId, setter) in NetworkDef.InputMsgDef.setter) {
                try {
                    setter(Collect(msgId), msg.Input);
                } catch (Exception ex) {
                    Log.Error("Exception when setting input msg {0}: {1}", msgId, ex.Message);
                }
            }
            return msg;
        }
        
        private IMessage Collect(MessageDef id) {
            if (!inputCollectors.ContainsKey(id)) {
                return null;
            }
            
            try {
                return inputCollectors[id].Collect();
            } catch (Exception e) {
                Log.Error(e.ToString());
                Log.Error("Exception when collecting input msg {0}: {1}", id, e.Message);
                return null;
            }
        }

        #endregion

        #region 接收
        
        public void RegisterHandler<T>(MessageDef id, Action<SortedDictionary<Uid, T>> handler) where T : IMessage {
            if (!NetworkUtils.CheckMessageType(id, typeof(T))) {
                return;
            }
            
            if (!inputHandlers.ContainsKey(id)) {
                inputHandlers.Add(id, new InputHandler<T>());
            }
            inputHandlers[id].Add(handler);
        }
        
        public void RemoveHandler<T>(MessageDef id, Action<SortedDictionary<Uid, T>> handler) where T : IMessage {
            if (!NetworkUtils.CheckMessageType(id, typeof(T))) {
                return;
            }
            
            if (!inputHandlers.ContainsKey(id)) {
                return;
            }
            inputHandlers[id].Remove(handler);
        }

        public void PushInputMsg(frame_input_s2c msg) {
            if (allInputs.ContainsKey(msg.Frame)) {
                Log.Warning("Duplicate frame input from server: " + msg.Frame);
                return;
            }
            allInputs.Add(msg.Frame, msg);
        }

        public bool FrameReady() {
            // TODO 当收到不连续帧时，加速表现层，而不是一帧内直接更新
            if (allInputs.ContainsKey(Frame + 1)) {
                Frame++;
                HandleFrameInputs(allInputs[Frame]);
                return true;
            }
            return false;
        }
        
        private void HandleFrameInputs(frame_input_s2c frameInput) {
            foreach (var (msgId, getter) in NetworkDef.InputMsgDef.getter) {
                if (!NetworkDef.InputMsgDef.creator.ContainsKey(msgId)) {
                    Log.Warning("NetworkDef.InputMsgDef.creator missing: " + msgId);
                    return;
                }
                IDictionary inputs = NetworkDef.InputMsgDef.creator[msgId].Invoke();
                foreach (var inputInfo in frameInput.Inputs) {
                    try {
                        var msg = getter(inputInfo.Input);
                        if (msg != null) {
                            inputs.Add(new Uid(inputInfo.Uid), msg);
                        }
                    } catch (Exception ex) {
                        Log.Error("Exception when getting input msg {0}: {1}", msgId, ex.Message);
                    }
                }
                Dispatch(msgId, inputs);
            }
        }
        
        private void Dispatch(MessageDef id, IDictionary inputs) {
            if (!inputHandlers.ContainsKey(id)) {
                return;
            }

            try {
                inputHandlers[id].Handle(inputs);
            } catch (Exception e) {
                Log.Error(e.ToString());
                Log.Error("Exception when handling input msg {0}: {1}", id, e.Message);
            }
        }

        #endregion
    }
}