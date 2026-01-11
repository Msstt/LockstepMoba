using Framework.UI;
using UI;
using UnityEngine;

public static class UIUtils {
    public static void ShowUI(UIDef def, IUIParam param = null) {
        GameMgr.Instance.GetSystem<IUISystem>()?.ShowUI(def, param);
    }
    
    public static void CloseUI(UIDef def) {
        GameMgr.Instance.GetSystem<IUISystem>()?.CloseUI(def);
    }

    public static void BindingUI(UIDef def, Transform transform, IUIParam param = null) {
        GameMgr.Instance.GetSystem<IUISystem>()?.BindingUI(def, transform, param);
    }

    public static void UnbindingUI(UIDef def, Transform transform) {
        GameMgr.Instance.GetSystem<IUISystem>()?.UnbindingUI(def, transform);
        
    }
}
