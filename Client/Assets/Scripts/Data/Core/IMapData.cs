using System.Collections.Generic;
using System.Linq;
using Framework;

namespace Data {
    public abstract class MapData<V> : Dictionary<int, V>, IData where V : ICheckableData, new() {
        public new V this[int key] {
            get {
                if (!TryGetValue(key, out var value)) {
                    value = new V();
                    base[key] = value;
                }
                return value;
            }
        }

        public int GetStatusCode() {
            int statusCode = StatusCode.Combine(StatusCode.Seed, Count);
            foreach (var pair in this.OrderBy(pair => pair.Key)) {
                statusCode = StatusCode.Combine(statusCode, pair.Key);
                statusCode = StatusCode.CombineData(statusCode, pair.Value);
            }
            return statusCode;
        }
    }
}
