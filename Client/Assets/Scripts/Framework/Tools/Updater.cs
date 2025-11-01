using System;
using UnityEngine;

namespace Framework {
    public class Updater : MonoSingleton<Updater> {
        private Action onUpdate = null;
        private Action onLateUpdate = null;
        
        public void RegisterUpdate(Action handler) {
            onUpdate += handler;
        }
        
        public void RegisterLateUpdate(Action handler) {
            onLateUpdate += handler;
        }
        
        public void RemoveUpdate(Action handler) {
            onUpdate -= handler;
        }
        
        public void RemoveLateUpdate(Action handler) {
            onLateUpdate -= handler;
        }
        
        public void Update() {
            try {
                onUpdate?.Invoke();
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
        }
        
        public void LateUpdate() {
            try {
                onLateUpdate?.Invoke();
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
        }
    }
}