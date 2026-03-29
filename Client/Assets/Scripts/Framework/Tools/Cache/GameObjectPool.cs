using UnityEngine;

namespace Framework {
    public class GameObjectPool {
        private ObjectPool<GameObject> cache;

        private GameObject prefab;
        private Transform parent;

        public GameObjectPool(GameObject prefab, GameObject parent) {
            this.prefab = prefab;
            this.parent = parent.transform;
            cache = new ObjectPool<GameObject>(CreateFunc, DestroyFunc);
            cache.SetRecycleFunc(RecycleFunc);
            cache.SetInitFunc(InitFunc);
        }
        
        public GameObject Get()
        {
            return cache.Get();
        }

        public void Recycle(GameObject go)
        {
            cache.Recycle(go);
        }
        
        public void Dispose()
        {
            cache.Dispose();
        }

        private GameObject CreateFunc() {
            GameObject go = Object.Instantiate(prefab, parent);
            return go;
        }
        
        private void DestroyFunc(GameObject go) {
            GameObject.Destroy(go);
        }
        
        private void RecycleFunc(GameObject go) {
            go.SetActive(false);
        }

        private void InitFunc(GameObject go) {
            go.SetActive(true);
        }
    }
}