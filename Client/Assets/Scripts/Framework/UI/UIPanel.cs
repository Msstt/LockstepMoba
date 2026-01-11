using UI;
using UnityEngine;

namespace Framework.UI {
    public interface IUIParam { }
    
    public abstract class UIPanel {
        public virtual void OnAwake() { }
        public virtual void OnShow(IUIParam param) { }
        public virtual void OnHide() { }
        public virtual void OnDestroy() { }
        
        private Transform transform = null;
        private UIDef def = UIDef.None;
        private UILayer layer = UILayer.None;

        public Transform Transform {
            get => transform;
            set {
                if (transform == null) {
                    transform = value;
                }
            }
        }
        
        public UIDef Def {
            get => def;
            set {
                if (def == UIDef.None) {
                    def = value;
                }
            }
        }
        
        public UILayer Layer {
            get => layer;
            set {
                if (layer == UILayer.None) {
                    layer = value;
                }
            }
        }

        protected void Close() {
            UIUtils.CloseUI(def);
        }
    }
}