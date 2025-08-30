using UnityEngine;

namespace Framework.Network {
    public static class Log {
        public static void Info(string message, params object[] args) {
            Debug.Log("[Network] [INFO] " + string.Format(message, args));
        }
        
        public static void Warning(string message, params object[] args) {
            Debug.LogWarning("[Network] [Warning] " + string.Format(message, args));
        }
        
        public static void Error(string message, params object[] args) {
            Debug.LogError("[Network] [Error] " + string.Format(message, args));
        }
    }
}