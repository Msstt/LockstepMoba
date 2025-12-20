using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework {
    public enum UnityEventType {
        OnUpdate,
        OnQuit,
    }
    
    public class UnityEventMgr : MonoSingleton<UnityEventMgr> {
        private Dictionary<UnityEventType, Action> listener = new();

        public void Awake() {
            for (int i = 0; i < Enum.GetValues(typeof(UnityEventType)).Length; i++) {
                listener[(UnityEventType)i] = null;
            }
        }
        
        public void Register(UnityEventType type, Action handler) {
            listener[type] += handler;
        }
        
        public void UnRegister(UnityEventType type, Action handler) {
            listener[type] -= handler;
        }
       
        public void Update() {
            try {
                listener[UnityEventType.OnUpdate]?.Invoke();
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
        }
        
        public void OnApplicationQuit() {
            try {
                listener[UnityEventType.OnQuit]?.Invoke();
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
        }
    }
}