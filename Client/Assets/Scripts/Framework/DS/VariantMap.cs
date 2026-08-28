using System.Collections.Generic;
using System.Linq;

namespace Framework {
    public class VariantMap {
        private Dictionary<string, object> map = new Dictionary<string, object>();
        
        public void Set<T>(string key, T value) {
            if (map.ContainsKey(key) && map[key] is not T) {
                throw new System.Exception($"VariantMap Set Key:{key} Type Mismatch. Existing Type:{map[key].GetType()} New Type:{typeof(T)}");
            }
            map[key] = value;
        }

        public T Get<T>(string key) {
            if (map.ContainsKey(key)) {
                if (map[key] is T value) {
                    return value;
                } else {
                    throw new System.Exception($"VariantMap Get Key:{key} Type Mismatch. Existing Type:{map[key].GetType()} Requested Type:{typeof(T)}");
                }
            }
            return default;
        }
        
        public T GetOrDefault<T>(string key, T defaultValue) {
            if (map.ContainsKey(key)) {
                if (map[key] is T value) {
                    return value;
                } else {
                    throw new System.Exception($"VariantMap GetOrDefault Key:{key} Type Mismatch. Existing Type:{map[key].GetType()} Requested Type:{typeof(T)}");
                }
            }
            return defaultValue;
        }

        public int GetStatusCode() {
            int code = StatusCode.Combine(StatusCode.Seed, map.Count);
            foreach (string key in map.Keys.OrderBy(key => key, System.StringComparer.Ordinal)) {
                code = StatusCode.Combine(code, key);
                code = StatusCode.CombineValue(code, map[key]);
            }
            return code;
        }
    }
}
