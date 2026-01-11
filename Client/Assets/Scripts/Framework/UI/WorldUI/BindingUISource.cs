// TODO: 显影时自动打开关闭界面

using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Framework.UI {
    public class BindingUISource : MonoBehaviour {
        private readonly Dictionary<UIDef, UIPanel> panels = new Dictionary<UIDef, UIPanel>();
        private Vector3 lastPos;
        private Vector3 cameraForward;

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
            panel.Transform.forward = UIUtils.UICamera.transform.forward;
        }

        public void LateUpdate() {
            if (transform.position == lastPos && UIUtils.UICamera.transform.forward == cameraForward) {
                return;
            }
            lastPos = transform.position;
            cameraForward = UIUtils.UICamera.transform.forward;
            foreach (var panel in panels.Values) {
                RefreshPanel(panel);
            }
        }
        
        public void OnDestroy() {
            while (panels.Count > 0) {
                var panel = panels.Values.GetEnumerator();
                panel.MoveNext();
                UIUtils.UnBindingUI(panel.Current.Def, transform);
            }
        }
    }
}