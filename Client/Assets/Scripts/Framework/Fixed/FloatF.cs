// 帧同步定点数，保留 6 位小数

using System;
using Newtonsoft.Json;

[JsonConverter(typeof(FloatFConverter))]
public struct FloatF : IComparable<FloatF> {
    public static FloatF eps = new FloatF(3, true);
    
    private const long scale = 1_000_000;
    
    [JsonProperty]
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

    public static FloatF FromFloat(double f) {
        return new FloatF((long)(f * scale), true);
    }
    public float ToFloat() {
        return 1.0f * value / scale;
    }
    public static implicit operator FloatF(string s) {
        if (!s.EndsWith("F") || !long.TryParse(s[..^1], out long f)) {
            throw new ArgumentException($"Cannot parse '{s}' to FloatF");
        }
        return new FloatF(f, true);
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
    public override string ToString() => value + "F";
    
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
    
        long n = x.value;
        long approx = n;
        for (int i = 0; i < 20; i++) {
            approx = (approx + n * scale / approx) / 2;
        }
        return new FloatF(approx, true);
    }

    #region Json Converter
    private class FloatFConverter : JsonConverter<FloatF> {
        public override void WriteJson(JsonWriter writer, FloatF value, JsonSerializer serializer) {
            writer.WriteValue(value.value + "F");
        }

        public override FloatF ReadJson(JsonReader reader, System.Type objectType, FloatF existingValue, bool hasExistingValue, JsonSerializer serializer) {
            if (reader.Value is string s && s.EndsWith("F")) {
                if (long.TryParse(s[..^1], out long f)) {
                    return new FloatF(f, true);
                }
            }

            throw new JsonSerializationException("FloatF Serialize failed");
        }
    }
    
    #endregion
}


