using System;
using System.Collections.Generic;

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
    }
}