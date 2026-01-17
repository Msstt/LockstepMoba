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
}