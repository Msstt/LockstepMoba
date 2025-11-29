// 帧同步 Vector3

using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct Vector3F {
    public FloatF x;
    public FloatF y;
    public FloatF z;

    public Vector3F(FloatF x, FloatF y, FloatF z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public static Vector3F FromVector3(Vector3 v) => new Vector3F(FloatF.FromFloat(v.x), FloatF.FromFloat(v.y), FloatF.FromFloat(v.z));
    public Vector3 ToVector3() => new Vector3(x.ToFloat(), y.ToFloat(), z.ToFloat());
    
    public static Vector3F operator+(Vector3F a, Vector3F b) => new Vector3F(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Vector3F operator-(Vector3F a, Vector3F b) => new Vector3F(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Vector3F operator*(Vector3F a, FloatF x) => new Vector3F(a.x * x, a.y * x, a.z * x);
    public static Vector3F operator/(Vector3F a, FloatF x) => new Vector3F(a.x / x, a.y / x, a.z / x);
    
    public static bool operator==(Vector3F a, Vector3F b) => a.x == b.x && a.y == b.y && a.z == b.z;
    public static bool operator!=(Vector3F a, Vector3F b) => a.x != b.x || a.y != b.y || a.z != b.z;
    
    public override bool Equals(object obj) => obj is Vector3F v && v == this;
    public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;

    public static FloatF Distance(Vector3F a, Vector3F b) {
        FloatF dx = a.x - b.x;
        FloatF dy = a.y - b.y;
        FloatF dz = a.z - b.z;
        return FloatF.Sqrt(dx * dx + dy * dy + dz * dz);
    }
    
    public static FloatF Distance2(Vector3F a, Vector3F b) {
        FloatF dx = a.x - b.x;
        FloatF dy = a.y - b.y;
        FloatF dz = a.z - b.z;
        return dx * dx + dy * dy + dz * dz;
    }
    
    public static FloatF Dot(Vector3F a, Vector3F b) {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
    
    public static Vector3F Cross(Vector3F a, Vector3F b) {
        return new Vector3F(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }
    
    public static Vector3F Mid(Vector3F a, Vector3F b) {
        return (a + b) / 2;
    }
    
    public static FloatF MaxDistance(Vector3F a, Vector3F b) {
        FloatF dx = FloatF.Abs(a.x - b.x);
        FloatF dy = FloatF.Abs(a.y - b.y);
        FloatF dz = FloatF.Abs(a.z - b.z);
        return FloatF.Max(FloatF.Max(dx, dy), dz);
    }
    
    public static bool IsEqualInEps(Vector3F a, Vector3F b) {
        return FloatF.Abs(a.x - b.x) <= FloatF.eps && FloatF.Abs(a.y - b.y) <= FloatF.eps && FloatF.Abs(a.z - b.z) <= FloatF.eps;
    }
    
    #region PropertyDrawer
    
    [CustomPropertyDrawer(typeof(Vector3F))]
    public class Float3Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var x = property.FindPropertyRelative("x");
            var y = property.FindPropertyRelative("y");
            var z = property.FindPropertyRelative("z");

            position = EditorGUI.PrefixLabel(position, label);

            float width = position.width / 3f;

            var r1 = new Rect(position.x + width * 0, position.y, width - 2, position.height);
            var r2 = new Rect(position.x + width * 1, position.y, width - 2, position.height);
            var r3 = new Rect(position.x + width * 2, position.y, width - 2, position.height);

            EditorGUI.PropertyField(r1, x, GUIContent.none);
            EditorGUI.PropertyField(r2, y, GUIContent.none);
            EditorGUI.PropertyField(r3, z, GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
    
    #endregion
}
