namespace Framework {
    public class ObjectPoolMgr<T> : Singleton<ObjectPoolMgr<T>> where T : new() {
        private ObjectPool<T> pool;
        
        public ObjectPoolMgr() {
            pool = new ObjectPool<T>(() => new T(), obj => { });
            pool.SetRecycleFunc(OnRecycle);
            pool.SetInitFunc(OnInit);
        }
        
        protected virtual void OnRecycle(T obj) { }
        protected virtual void OnInit(T obj) { }

        public T Get() {
            return pool.Get();
        }
        
        public void Recycle(T obj) {
            pool.Recycle(obj);
        }
    }
}