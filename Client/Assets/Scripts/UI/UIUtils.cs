using System;
using Framework.UI;
using UI;
using UnityEngine;

public static class UIUtils {
    public static Camera UICamera => GameMgr.Instance.GetSystem<IUISystem>()?.UICamera;

    public static void ShowUI(UIDef def, IUIParam param = null) {
        GameMgr.Instance.GetSystem<IUISystem>()?.ShowUI(def, param);
    }
    
    public static void CloseUI(UIDef def) {
        GameMgr.Instance.GetSystem<IUISystem>()?.CloseUI(def);
    }

    public static void BindingUI(UIDef def, Transform transform, IUIParam param = null) {
        GameMgr.Instance.GetSystem<IUISystem>()?.BindingUI(def, transform, param);
    }

    public static void UnBindingUI(UIDef def, Transform transform) {
        GameMgr.Instance.GetSystem<IUISystem>()?.UnBindingUI(def, transform);
    }

    public static void InitChildCount(GameObject root, GameObject prefab, int count, Action<int, GameObject> func) {
        int childCount = root.transform.childCount;
        for (int i = 0; i < count; i++) {
            GameObject node;
            if (i < childCount) {
                node = root.transform.GetChild(i).gameObject;
            } else {
                node = GameObject.Instantiate(prefab, root.transform);
            }
            func(i, node);
        }
        for (int i = count; i < childCount; i++) {
            root.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
