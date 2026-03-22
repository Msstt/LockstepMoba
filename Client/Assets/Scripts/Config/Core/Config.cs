// TODO 预加载

using System;
using System.Collections.Generic;

public static partial class Config {
    public class ConfigCache<T> {
        private Dictionary<int, T> cache = new Dictionary<int, T>();
        private Func<int, T> loader;

        public ConfigCache(Func<int, T> loader) {
            if (loader == null) {
                throw new ArgumentNullException(nameof(loader));
            }
            this.loader = loader;
        }
    
        public T this[int id] {
            get {
                if (cache.ContainsKey(id)) {
                    return cache[id];
                }
                T config = loader(id);
                if (config == null) { 
                    throw new Exception($"{typeof(T)} config not found: {id}");
                }
                return cache[id] = config;
            }
        }
    }
}
