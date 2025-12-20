// 帧同步 Vector3

using System;
using Sirenix.OdinInspector.Editor;
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
    public override string ToString() => $"({x}, {y}, {z})";

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
    
    public static bool IsEqualInEps(Vector3F a, Vector3F b, FloatF eps) {
        return FloatF.Abs(a.x - b.x) <= eps && FloatF.Abs(a.y - b.y) <= eps && FloatF.Abs(a.z - b.z) <= eps;
    }
}

public class Vector3FDrawer : OdinValueDrawer<Vector3F> {
    protected override void DrawPropertyLayout(GUIContent label) {
        Vector3F v = ValueEntry.SmartValue;

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
