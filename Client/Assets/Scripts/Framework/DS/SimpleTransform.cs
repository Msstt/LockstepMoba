using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

[Serializable]
public class SimpleTransform {
    public Vector3F position;
    public FloatF direction;
}

#if UNITY_EDITOR
public class SimpleTransformDrawer : OdinValueDrawer<SimpleTransform> {
    protected override void DrawPropertyLayout(GUIContent label) {
        SimpleTransform t = ValueEntry.SmartValue;

        EditorGUILayout.BeginHorizontal();

        if (label != null) {
            EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth));
        }

        DrawInput(" x ", ref t.position.x, FloatF.one);
        DrawInput(" y ", ref t.position.y, FloatF.one);
        DrawInput(" z ", ref t.position.z, FloatF.one);
        DrawInput(" d ", ref t.direction, FloatF.pi / 180);
        
        EditorGUILayout.EndHorizontal();

        ValueEntry.SmartValue = t;
    }

    private void DrawInput(string name, ref FloatF value, FloatF scale) {
        EditorGUILayout.LabelField(name, GUILayout.Width(15));
        string input = EditorGUILayout.TextField((value / scale).ToString());
        value = FloatF.Parse(input) * scale ?? value;
    }
}
#endif