using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

namespace Combat.Actor {
    [Serializable]
    public class LevelNumber<T> {
        [Serializable]
        public class Data {
            [HorizontalGroup(0.3f)]
            [LabelText("等级")]
            [LabelWidth(40)]
            public int level;

            [HorizontalGroup(0.7f)]
            [HideLabel]
            public T value;
        }
        
        public List<Data> datas = new List<Data>();
        
        public T this[int level] {
            get {
                T ret = default;
                foreach (var data in datas) {
                    if (data.level <= level) {
                        ret = data.value;
                    }
                }
                return ret;
            }
        }
    }
    
#if UNITY_EDITOR
    public class LevelNumberDrawer<T> : OdinValueDrawer<LevelNumber<T>> {
        protected override void DrawPropertyLayout(GUIContent label) {
            LevelNumber<T> v = ValueEntry.SmartValue;

            EditorGUILayout.BeginHorizontal();

            if (label != null) {
                EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth));
            }

            foreach (var property in Property.Children) {
                property.Draw();
            }
        
            EditorGUILayout.EndHorizontal();

            ValueEntry.SmartValue = v;
        }
    }
#endif
}
