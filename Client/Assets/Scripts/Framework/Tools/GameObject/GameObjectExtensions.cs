using UnityEngine;

public static class GameObjectExtensions {
    public static T EnsureComponent<T>(this GameObject go) where T : Component {
        var com = go.GetComponent<T>();
        if (com == null) {
            com = go.AddComponent<T>();
        }
        return com;
    }
    
    public static T EnsureComponent<T>(this Transform trans) where T : Component {
        var com = trans.GetComponent<T>();
        if (com == null) {
            com = trans.gameObject.AddComponent<T>();
        }
        return com;
    }
    
    public static T GetComponent<T>(this Transform trans, string path) where T : Component {
        return trans?.Find(path)?.GetComponent<T>();
    }
    
    public static T GetComponent<T>(this GameObject trans, string path) where T : Component {
        return trans?.transform.Find(path)?.GetComponent<T>();
    }
    
    public static GameObject GetGameObject(this Transform trans, string path) {
        return trans?.Find(path)?.GetComponent<Transform>().gameObject;
    }
    
    public static GameObject GetGameObject(this GameObject trans, string path) {
        return trans?.transform.Find(path)?.GetComponent<Transform>().gameObject;
    }
    
    public static void DestroyAllChildren(this GameObject trans) {
        for (int i = trans.transform.childCount - 1; i >= 0; i--) {
            GameObject.Destroy(trans.transform.GetChild(i).gameObject);
        }
    }
    
    public static void SetVisible(this GameObject go, bool visible) {
        foreach (var coms in go.GetComponentsInChildren<Renderer>()) {
            coms.enabled = visible;
        }
        
        foreach (var coms in go.GetComponentsInChildren<Light>()) {
            coms.enabled = visible;
        }
        
        foreach (var coms in go.GetComponentsInChildren<AudioSource>()) {
            coms.enabled = visible;
        }
        
        foreach (var coms in go.GetComponentsInChildren<Canvas>()) {
            coms.enabled = visible;
        }
        
        foreach (var coms in go.GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.EmissionModule emission = coms.emission;
            emission.enabled = visible;
        }
    }
}