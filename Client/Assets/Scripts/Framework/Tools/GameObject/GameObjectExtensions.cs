using UnityEngine;

public static class GameObjectExtensions {
    public static T EnsureComponent<T>(this GameObject go) where T : Component {
        var com = go.GetComponent<T>();
        if (com == null) {
            com = go.AddComponent<T>();
        }
        return com;
    }
    
    public static T EnsureComponent<T>(this Transform go) where T : Component {
        var com = go.GetComponent<T>();
        if (com == null) {
            com = go.gameObject.AddComponent<T>();
        }
        return com;
    }
}