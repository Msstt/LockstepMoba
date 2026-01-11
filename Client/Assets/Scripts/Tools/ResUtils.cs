using UnityEngine;

public static class ResUtils {
    public static T Load<T>(string prefabName) where T : Object {
        return Resources.Load<T>(prefabName);
    }
}
