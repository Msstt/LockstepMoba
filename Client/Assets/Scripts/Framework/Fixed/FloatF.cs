// 帧同步定点数，保留 6 位小数

using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

[Serializable, JsonConverter(typeof(FloatFConverter))]
public struct FloatF : IComparable<FloatF> {
    public static FloatF eps = new FloatF(3, true);
    public static FloatF zero = new FloatF(0, true);
    public static FloatF max = new FloatF(long.MaxValue, true);
    
    public const long scale = 1_000_000;
    
    [SerializeField, JsonProperty]
    private long value;
    
    public FloatF(long f, bool isRaw = false) {
        if (isRaw) {
            value = f;
        } else {
            value = f * scale;
        }
    }
    
    public static implicit operator FloatF(long f) => new FloatF(f);
    public static explicit operator int(FloatF f) => (int)(f.value / scale);
    
    public float ToFloat() => 1.0f * value / scale;
    public static implicit operator FloatF(string s) {
        FloatF? f = Parse(s);
        if (f.HasValue) {
            return f.Value;
        }
        throw new ArgumentException($"Cannot parse '{s}' to FloatF");
    }

    public static FloatF operator+(FloatF a, FloatF b) => new FloatF(a.value + b.value, true);
    public static FloatF operator-(FloatF a, FloatF b) => new FloatF(a.value - b.value, true);
    public static FloatF operator*(FloatF a, FloatF b) => new FloatF(a.value * b.value / scale, true);
    public static FloatF operator/(FloatF a, FloatF b) => new FloatF(a.value * scale / b.value, true);
    
    public static bool operator>(FloatF a, FloatF b) => a.value > b.value;
    public static bool operator<(FloatF a, FloatF b) => a.value < b.value;
    public static bool operator>=(FloatF a, FloatF b) => a.value >= b.value;
    public static bool operator<=(FloatF a, FloatF b) => a.value <= b.value;
    public static bool operator==(FloatF a, FloatF b) => a.value == b.value;
    public static bool operator!=(FloatF a, FloatF b) => a.value != b.value;
    public int CompareTo(FloatF other) => value.CompareTo(other.value);
    public static FloatF Max(FloatF a, FloatF b) => a > b ? a : b;
    public static FloatF Min(FloatF a, FloatF b) => a < b ? a : b;
    
    public override bool Equals(object obj) => obj is FloatF f && f.value == value;
    public override int GetHashCode() => value.GetHashCode();

    public override string ToString() {
        return (value < 0 ? "-" + -value / scale : value / scale) + (value % scale != 0 ? "." + Math.Abs(value % scale).ToString("D6").TrimEnd('0') : "");
    }
    
    public static FloatF? Parse(string s) {
        if (string.IsNullOrEmpty(s)) {
            return null;
        }
        
        string[] parts = s.Split('.');
        if (parts.Length > 2) {
            return null;
        }

        if (!long.TryParse(parts[0], out long intPart)) {
            return null;
        }

        long fracPart = 0;
        if (parts.Length == 2) {
            string fracStr = parts[1].PadRight(6, '0');
            if (fracStr.Length > 6 || !long.TryParse(fracStr, out fracPart)) {
                return null;
            }
        }

        return new FloatF(intPart * scale + (s[0] == '-' ? -fracPart : fracPart), true);
    }
    
    public static FloatF Abs(FloatF a) {
        if (a.value < 0) {
            return new FloatF(-a.value, true);
        } else {
            return new FloatF(a.value, true);
        }
    }
    public static FloatF Sqrt(FloatF x) {
        if (x.value < 0) {
            throw new ArgumentException($"Cannot sqrt negative FloatF: {x}");
        }

        if (x.value == 0) {
            return zero;
        }
    
        long n = x.value;
        long approx = n;
        for (int i = 0; i < 20; i++) {
            approx = (approx + n * scale / approx) / 2;
        }
        return new FloatF(approx, true);
    }

    public static int FloorInt(FloatF x) {
        return (int)(x.value / scale);
    }
    
    public Network.float_f ToProto() {
        return new Network.float_f { Value = value };
    }
}

#region Json Converter
public class FloatFConverter : JsonConverter<FloatF> {
    public override void WriteJson(JsonWriter writer, FloatF value, JsonSerializer serializer) {
        writer.WriteValue(value.ToString());
    }

    public override FloatF ReadJson(JsonReader reader, Type objectType, FloatF existingValue, bool hasExistingValue, JsonSerializer serializer) {
        if (reader.Value is string s) {
            FloatF? f = FloatF.Parse(s);
            if (f.HasValue) {
                return f.Value;
            }
        }

        throw new JsonSerializationException("FloatF Serialize failed");
    }
}
    
#endregion

#region PropertyDrawer

public class FloatFPropertyDrawer : OdinValueDrawer<FloatF> {
    protected override void DrawPropertyLayout(GUIContent label) {
        FloatF f = ValueEntry.SmartValue;

        EditorGUILayout.BeginHorizontal();

        if (label != null) {
            Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);
        }

        string input = EditorGUILayout.TextField(f.ToString());
        f = FloatF.Parse(input)??f;

        EditorGUILayout.EndHorizontal();

        ValueEntry.SmartValue = f;
    }
}

#endregion

public static class FloatFProtoExtensions {
    public static FloatF ToFloatF(this Network.float_f msg) => new FloatF(msg.Value, true);
    
    public static FloatF ToFloatF(this double f) => new FloatF((long)(f * FloatF.scale), true);
    public static FloatF ToFloatF(this float f) => new FloatF((long)(f * FloatF.scale), true);
}