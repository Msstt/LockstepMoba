using System;
using System.Collections.Generic;

namespace Framework {
    public class ListPool<T> : Singleton<ListPool<T>> {
        
        private ObjectPool<PooledList<T>> pool;
        
        public ListPool() {
            pool = new ObjectPool<PooledList<T>>(() => new PooledList<T>(), obj => { });
            pool.SetRecycleFunc(obj => obj.Clear());
        }

        public PooledList<T> Get() {
            return pool.Get();
        }
        
        public void Recycle(PooledList<T> obj) {
            pool.Recycle(obj);
        }
    }
    
    public class PooledList<T> : List<T>, IDisposable {
        public static PooledList<T> Get() {
            return ListPool<T>.Instance.Get();
        }
        
        public void Dispose() {
            ListPool<T>.Instance.Recycle(this);
        }
    }
}