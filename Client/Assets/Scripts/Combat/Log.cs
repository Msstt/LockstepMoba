using UnityEngine;

namespace Combat {
    public static class Log {
        public static void Info(string message, params object[] args) {
            Debug.Log("[Combat] [INFO] " + GameMgr.Instance.Frame + ": " + string.Format(message, args));
        }
        
        public static void Warning(string message, params object[] args) {
            Debug.LogWarning("[Combat] [Warning] " + GameMgr.Instance.Frame + ": " + string.Format(message, args));
        }
        
        public static void Error(string message, params object[] args) {
            Debug.LogError("[Combat] [Error] " + GameMgr.Instance.Frame + ": " + string.Format(message, args));
        }
    }
    
    public class CombatException : System.Exception {
        public CombatException(string message) : base(message) { }
    }
}