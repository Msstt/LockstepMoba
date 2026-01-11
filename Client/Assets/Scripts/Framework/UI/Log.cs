using UnityEngine;

namespace Framework.UI {
    public static class Log {
        public static void Info(string message, params object[] args) {
            Debug.Log("[UI] [INFO] " + string.Format(message, args));
        }
        
        public static void Warning(string message, params object[] args) {
            Debug.LogWarning("[UI] [Warning] " + string.Format(message, args));
        }
        
        public static void Error(string message, params object[] args) {
            Debug.LogError("[UI] [Error] " + string.Format(message, args));
        }
    }
    
    public class UIException : System.Exception {
        public UIException(string message) : base(message) { }
    }
}