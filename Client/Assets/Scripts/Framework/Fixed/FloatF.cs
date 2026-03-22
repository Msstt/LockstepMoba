// 帧同步定点数，保留 6 位小数

using System;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

[Serializable, JsonConverter(typeof(FloatFConverter))]
public struct FloatF : IComparable<FloatF> {
    public static FloatF eps = new FloatF(3, true);
    public static FloatF zero = new FloatF(0, true);
    public static FloatF one = new FloatF(1);
    public static FloatF max = new FloatF(long.MaxValue, true);
    public static FloatF pi = new FloatF(3141592, true);
    
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
    public static FloatF operator-(FloatF a) => new FloatF(-a.value, true);
    public static FloatF operator*(FloatF a, FloatF b) => new FloatF(a.value * b.value / scale, true);
    public static FloatF operator/(FloatF a, FloatF b) => new FloatF(a.value * scale / b.value, true);
    public static FloatF operator%(FloatF a, FloatF b) => new FloatF(a.value % b.value, true);
    
    public static bool operator>(FloatF a, FloatF b) => a.value > b.value;
    public static bool operator<(FloatF a, FloatF b) => a.value < b.value;
    public static bool operator>=(FloatF a, FloatF b) => a.value >= b.value;
    public static bool operator<=(FloatF a, FloatF b) => a.value <= b.value;
    public static bool operator==(FloatF a, FloatF b) => a.value == b.value;
    public static bool operator!=(FloatF a, FloatF b) => a.value != b.value;
    public int CompareTo(FloatF other) => value.CompareTo(other.value);
    public static FloatF Max(FloatF a, FloatF b) => a > b ? a : b;
    public static FloatF Min(FloatF a, FloatF b) => a < b ? a : b;

    public override bool Equals(object obj) {
        if (obj is FloatF f) return f.value == value;
        if (obj is byte or sbyte or ushort or int or uint or long or ulong) {
            long v = Convert.ToInt64(obj);
            return v * scale == value;
        }
        return false;
    }
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

    public static FloatF Clamp(FloatF value, FloatF min, FloatF max) {
        return Min(Max(value, min), max);
    }

    // TODO 精度可能还是不够
    public static FloatF Sin(FloatF value) {
        value %= 2 * pi;
        if (value > pi) {
            value -= 2 * pi;
        } else if (value < -pi) {
            value += 2 * pi;
        }

        if (value > pi / 2) {
            value = pi - value;
        } else if (value < -pi / 2) {
            value = -pi - value;
        }
        
        FloatF value2 = value * value;
        return value - (value * value2) / 6 + (value * value2 * value2) / 120;
    }

    public static FloatF Cos(FloatF value) {
        return Sin(value + pi / 2);
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
        f = FloatF.Parse(input) ?? f;

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