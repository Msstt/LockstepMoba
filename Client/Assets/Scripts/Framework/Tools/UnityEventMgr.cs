using System;
using UnityEngine;

namespace Framework {
    public class UnityEventMgr : MonoSingleton<UnityEventMgr> {
        #region 退出应用

        private Action onQuit;
        
        public void RegisterOnQuit(Action handler) {
            onQuit += handler;
        }
        
        public void RemoveOnQuit(Action handler) {
            onQuit -= handler;
        }
        
        public void OnApplicationQuit() {
            try {
                onQuit?.Invoke();
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
        }

        #endregion
        
    }
}