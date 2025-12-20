// 全局事件

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework {
    public class EventMgr : Singleton<EventMgr> {
        
        private Dictionary<Type, Delegate> eventListeners = new Dictionary<Type, Delegate>();
        private Dictionary<Type, Delegate> noParamEventListeners = new Dictionary<Type, Delegate>();
        
        public void Register<T>(Action listener) where T : struct {
            Type type = typeof(T);
            if (!noParamEventListeners.ContainsKey(type)) {
                noParamEventListeners.Add(type, null);
            }
            noParamEventListeners[type] = Delegate.Combine(noParamEventListeners[type], listener);
        }
        
        public void UnRegister<T>(Action listener) where T : struct {
            Type type = typeof(T);
            if (!noParamEventListeners.ContainsKey(type)) {
                return;
            }
            noParamEventListeners[type] = Delegate.Remove(noParamEventListeners[type], listener);
        }
        
        public void Register<T>(Action<T> listener) where T : struct {
            Type type = typeof(T);
            if (!eventListeners.ContainsKey(type)) {
                eventListeners.Add(type, null);
            }
            eventListeners[type] = Delegate.Combine(eventListeners[type], listener);
        }
        
        public void UnRegister<T>(Action<T> listener) where T : struct {
            Type type = typeof(T);
            if (!eventListeners.ContainsKey(type)) {
                return;
            }
            eventListeners[type] = Delegate.Remove(eventListeners[type], listener);
        }
        
        public void Send<T>(T param) where T : struct {
            Type type = typeof(T);
            Action<T> listener = null;
            Action noParamListener = null;
            if (eventListeners.ContainsKey(type)) {
                listener = eventListeners[type] as Action<T>;
            }
            if (noParamEventListeners.ContainsKey(type)) {
                noParamListener = noParamEventListeners[type] as Action;
            }
            try {
                noParamListener?.Invoke();
                listener?.Invoke(param);
            } catch (Exception e) {
                Debug.LogError($"[EventMgr] Error calling {param.ToString()}: {e}");
            }
        }
    }
}
