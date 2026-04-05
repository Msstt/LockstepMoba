using UnityEngine;

public static class GoUtils {
    public static GameObject NewGo(string prefabName, Transform parent, bool isInit = false) {
        GameObject prefab = ResUtils.Load<GameObject>(prefabName);
        if (prefab == null) {
            Debug.LogError(prefabName + " is missing!");
            return null;
        }
        GameObject go = Object.Instantiate(prefab);
        
        if (parent != null) {
            go.transform.SetParent(parent, false);
        }
        
        if (isInit) {
            go.transform.localPosition = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }
        return go;
    }

    public static void SetGoActive(Transform go, string path, bool active) {
        go?.Find(path)?.gameObject.SetActive(active);
    }
    
    public static void SetGoActive(GameObject go, string path, bool active) {
        go?.transform.Find(path)?.gameObject.SetActive(active);
    }
}
