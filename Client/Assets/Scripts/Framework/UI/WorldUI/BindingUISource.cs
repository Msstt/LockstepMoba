// TODO: 显影时自动打开关闭界面

using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Framework.UI {
    public class BindingUISource : MonoBehaviour {
        private readonly Dictionary<UIDef, UIPanel> panels = new Dictionary<UIDef, UIPanel>();
        private Vector3 lastPos;

        public UIPanel GetPanel(UIDef def) {
            return panels.GetValueOrDefault(def, null);
        }
        
        public void AddPanel(UIPanel panel) {
            if (panel == null) {
                return;
            }
            panels.TryAdd(panel.Def, panel);
            RefreshPanel(panel);
        }
        
        public void RemovePanel(UIDef def) {
            panels.Remove(def);
        }

        private void RefreshPanel(UIPanel panel) {
            panel.Transform.position = transform.position;
        }

        public void Update() {
            if (transform.position == lastPos) {
                return;
            }
            lastPos = transform.position;
            foreach (var panel in panels.Values) {
                RefreshPanel(panel);
            }
        }

        public void OnDestroy() {
            foreach (var panel in panels.Values) {
                UIUtils.UnbindingUI(panel.Def, transform);
            }
            panels.Clear();
        }
    }
}