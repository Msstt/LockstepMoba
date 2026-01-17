using UI;
using UnityEditor;
using UnityEngine;

namespace UI {
    public enum BarType {
        LeftToRight,
        BottomToTop,
    }
    
    public class UIBarCom : MonoBehaviour {
        public BarType type = BarType.LeftToRight;
        
        private float value = 0f;
        
        public float Value {
            get => value;
            set {
                this.value = Mathf.Clamp01(value);
                Refresh();
            }
        }
        
        private void Refresh() {
            RectTransform child = transform.GetChild(0).GetComponent<RectTransform>();
            child.offsetMin = Vector2.zero;
            child.offsetMax = Vector2.zero;
            switch (type) {
                case BarType.LeftToRight:
                    child.anchorMin = Vector2.zero;
                    child.anchorMax = new Vector2(value, 1);
                    break;
                default:
                    child.anchorMin = Vector2.zero;
                    child.anchorMax = Vector2.one;
                    break;
            }
        }
    }
    
    [CustomEditor(typeof(UIBarCom))]
    public class UIBarComEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
