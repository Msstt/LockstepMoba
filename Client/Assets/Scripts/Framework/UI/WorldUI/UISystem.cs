using UI;
using UnityEngine;

namespace Framework.UI {
    public partial class UISystem : IUISystem {
        public void BindingUI(UIDef def, Transform transform, IUIParam param = null) {
            if (transform == null) {
                Log.Error("BindingUI transform is null: " + def);
                return;
            }
            BindingUISource source = transform.EnsureComponent<BindingUISource>();
            UIPanel panel = source.GetPanel(def);
            if (panel == null) {
                if (!UIConfig.config.TryGetValue(def, out UIConfig.Info config)) {
                    throw new UIException("UISystem panel config not found: " + def);
                }
                if (config.layer != UILayer.World) {
                    Log.Error("BindingUI only support World Layer: " + def);
                    return;
                }
                panel = CreatePanel(def, config, worldCanvas);
                if (panel == null) {
                    return;
                }
                ExecuteOnAwake(panel);
                source.AddPanel(panel);
            }
            ExecuteOnShow(panel, param);
        }
        
        public void UnBindingUI(UIDef def, Transform transform) {
            if (transform == null) {
                Log.Error("UnBindingUI transform is null: " + def);
                return;
            }
            BindingUISource source = transform.EnsureComponent<BindingUISource>();
            UIPanel panel = source.GetPanel(def);
            if (panel == null) {
                return;
            }
            ExecuteOnHide(panel);
            ExecuteOnDestroy(panel);
            GameObject.Destroy(panel.Transform.gameObject);
            source.RemovePanel(def);
        }
    }
}