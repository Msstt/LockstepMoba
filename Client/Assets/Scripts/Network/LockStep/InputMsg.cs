using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Google.Protobuf;

namespace Network {
    public interface IInputHandler {
        public void Handle(IDictionary msg);
        public void Add(Delegate handler);
        public void Remove(Delegate handler);
    }

    public class InputHandler<T> : IInputHandler where T : IMessage {
        public Action<Dictionary<Uid, T>> handlers = null;
        
        public void Handle(IDictionary iMsg) {
            if (iMsg is not Dictionary<Uid, T> msg) {
                return;
            }
            handlers?.Invoke(msg);
        }
        
        public void Add(Delegate iHandler) {
            if (iHandler is not Action<Dictionary<Uid, T>> handler) {
                return;
            }
            handlers += handler;
        }
        
        public void Remove(Delegate iHandler) {
            if (iHandler is not Action<Dictionary<Uid, T>> handler) {
                return;
            }
            handlers -= handler;
        }
    }
    
    
    public interface IInputCollector {
        public IMessage Collect();
        public void Add(Delegate collector);
        public void Remove(Delegate collector);
    }

    public class InputCollector<T> : IInputCollector where T : IMessage, new() {
        public Func<T> collectors = null;
        
        public IMessage Collect() {
            return collectors != null ? collectors.Invoke() : new T();
        }
        
        public void Add(Delegate iCollector) {
            if (iCollector is not Func<T> collector) {
                return;
            }
            collectors += collector;
        }
        
        public void Remove(Delegate iCollector) {
            if (iCollector is not Func<T> collector) {
                return;
            }
            collectors -= collector;
        }
    }
}