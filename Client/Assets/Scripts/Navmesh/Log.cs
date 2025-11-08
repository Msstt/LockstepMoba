using UnityEngine;

namespace Navmesh {
    public static class Log {
        public static void Info(string message, params object[] args) {
            Debug.Log("[Navmesh] [INFO] " + string.Format(message, args));
        }
        
        public static void Warning(string message, params object[] args) {
            Debug.LogWarning("[Navmesh] [Warning] " + string.Format(message, args));
        }
        
        public static void Error(string message, params object[] args) {
            Debug.LogError("[Navmesh] [Error] " + string.Format(message, args));
        }
    }
}