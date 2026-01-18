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
        public float fadeSpeed = 0.5f;
        
        private float value = 0f;
        private float fadeValue = 0f;
        
        public float Value {
            get => value;
            set {
                this.value = Mathf.Clamp01(value);
                Refresh(1, value);
            }
        }
        
        private void Refresh(int index, float v) {
            RectTransform child = transform.GetChild(index).GetComponent<RectTransform>();
            child.offsetMin = Vector2.zero;
            child.offsetMax = Vector2.zero;
            switch (type) {
                case BarType.LeftToRight:
                    child.anchorMin = Vector2.zero;
                    child.anchorMax = new Vector2(v, 1);
                    break;
                default:
                    child.anchorMin = Vector2.zero;
                    child.anchorMax = Vector2.one;
                    break;
            }
        }

        public void Update() {
            if (fadeValue < value) {
                fadeValue = value;
                Refresh(0, fadeValue);
            } else if (fadeValue > value) {
                fadeValue -= Time.deltaTime * fadeSpeed;
                Refresh(0, fadeValue);
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
