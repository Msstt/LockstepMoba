using UnityEngine;

namespace Combat {
    public static class Log {
        public static void Info(string message, params object[] args) {
            Debug.Log("[Combat] [INFO] " + string.Format(message, args));
        }
        
        public static void Warning(string message, params object[] args) {
            Debug.LogWarning("[Combat] [Warning] " + string.Format(message, args));
        }
        
        public static void Error(string message, params object[] args) {
            Debug.LogError("[Combat] [Error] " + string.Format(message, args));
        }
    }
}