// 简易对象池，没有保证不能重复回收同一个对象

using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework {
    public class ObjectPool<T> : IDisposable {
        public delegate T CreateFunc();
        public delegate void DestroyFunc(T obj);
        public delegate void RecycleFunc(T obj);
        public delegate void InitFunc(T obj);
        
        private Stack<T> cache = new Stack<T>();

        private CreateFunc createFunc;
        private DestroyFunc destroyFunc;
        private RecycleFunc recycleFunc;
        private InitFunc initFunc;
        
        public ObjectPool(CreateFunc createFunc, DestroyFunc destroyFunc) {
            if (createFunc == null || destroyFunc == null) {
                throw new ArgumentNullException("CreateFunc and DestroyFunc cannot be null.");
            }
            this.createFunc = createFunc;
            this.destroyFunc = destroyFunc;
        }
        
        public void SetRecycleFunc(RecycleFunc recycleFunc) {
            this.recycleFunc = recycleFunc;
        }
        
        public void SetInitFunc(InitFunc initFunc) {
            this.initFunc = initFunc;
        }

        public T Get() {
            T obj = cache.Any() ? cache.Pop() : createFunc();
            initFunc?.Invoke(obj);
            return obj;
        }

        public void Recycle(T obj) {
            recycleFunc?.Invoke(obj);
            cache.Push(obj);
        }

        public void Dispose() {
            foreach (var obj in cache) {
                destroyFunc(obj);
            }
            cache.Clear();
        }
    }
}