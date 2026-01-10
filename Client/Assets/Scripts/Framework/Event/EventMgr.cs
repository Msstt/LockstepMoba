// 全局事件

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework {
    public class EventMgr : Singleton<EventMgr> {
        
        private Dictionary<Type, ISafeEvent> eventListeners = new Dictionary<Type, ISafeEvent>();
        private Dictionary<Type, ISafeEvent> noParamEventListeners = new Dictionary<Type, ISafeEvent>();
        
        public void Register<T>(Action listener) where T : struct {
            Type type = typeof(T);
            if (!noParamEventListeners.ContainsKey(type)) {
                noParamEventListeners.Add(type, new SafeEvent());
            }
            (noParamEventListeners[type] as SafeEvent)?.Register(listener);
        }
        
        public void UnRegister<T>(Action listener) where T : struct {
            Type type = typeof(T);
            if (!noParamEventListeners.ContainsKey(type)) {
                return;
            }
            (noParamEventListeners[type] as SafeEvent)?.UnRegister(listener);
        }
        
        public void Register<T>(Action<T> listener) where T : struct {
            Type type = typeof(T);
            if (!eventListeners.ContainsKey(type)) {
                eventListeners.Add(type, new  SafeEvent<T>());
            }
            (eventListeners[type] as SafeEvent<T>)?.Register(listener);
        }
        
        public void UnRegister<T>(Action<T> listener) where T : struct {
            Type type = typeof(T);
            if (!eventListeners.ContainsKey(type)) {
                return;
            }
            (eventListeners[type] as SafeEvent<T>)?.UnRegister(listener);
        }
        
        public void Send<T>(T param) where T : struct {
            Type type = typeof(T);
            SafeEvent<T> listener = null;
            SafeEvent noParamListener = null;
            if (eventListeners.ContainsKey(type)) {
                listener = eventListeners[type] as SafeEvent<T>;
            }
            if (noParamEventListeners.ContainsKey(type)) {
                noParamListener = noParamEventListeners[type] as SafeEvent;
            }
            try {
                noParamListener?.Send();
                listener?.Send(param);
            } catch (Exception e) {
                Debug.LogError($"[EventMgr] Error calling {param.ToString()}: {e}");
            }
        }
    }
}
