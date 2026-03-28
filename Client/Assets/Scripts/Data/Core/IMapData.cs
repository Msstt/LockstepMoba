using System.Collections.Generic;

namespace Data {
    public abstract class MapData<V> : Dictionary<int, V>, IData where V : new() {
        public new V this[int key] {
            get {
                if (!TryGetValue(key, out var value)) {
                    value = new V();
                    base[key] = value;
                }
                return value;
            }
        }
    }
}