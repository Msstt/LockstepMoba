using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace Network {
    public interface ILockStep : IStartSystem, IFrameDriver {
        public void PushInputMsg(frame_input_s2c msg);
        public frame_input_c2s GetInputMsg();

        public void RegisterCollector<T>(MessageDef id, Func<T> collector) where T : IMessage, new();
        public void RemoveCollector<T>(MessageDef id, Func<T> collector) where T : IMessage, new();

        public void RegisterHandler<T>(MessageDef id, Action<SortedDictionary<Uid, T>> handler) where T : IMessage;
        public void RemoveHandler<T>(MessageDef id, Action<SortedDictionary<Uid, T>> handler) where T : IMessage;
    }
}