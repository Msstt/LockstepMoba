using System;
using System.Collections.Generic;
using System.Linq;
using Framework;

namespace Data {
    public class DataSystem : IDataSystem {
        private Dictionary<Type, IData> data = new Dictionary<Type, IData>();

        public T Get<T>() where T : class, IData, new() {
            Type type = typeof(T);
            if (!data.TryGetValue(type, out var value)) {
                value = new T();
                data[type] = value;
            }
            return value as T;
        }

        public int GetStatusCode() {
            int statusCode = StatusCode.Combine(StatusCode.Seed, data.Count);
            foreach (var pair in data.OrderBy(pair => pair.Key.FullName, System.StringComparer.Ordinal)) {
                statusCode = StatusCode.CombineType(statusCode, pair.Key);
                statusCode = StatusCode.Combine(statusCode, pair.Value.GetStatusCode());
            }
            return statusCode;
        }
    }
}
