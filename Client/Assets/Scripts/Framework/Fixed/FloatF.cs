// 帧同步定点数，保留 6 位小数

using System;
using Newtonsoft.Json;

[JsonConverter(typeof(FloatFConverter))]
public struct FloatF {
    private const int scale = 1000000;
    
    [JsonProperty]
    private int value;

    public FloatF(float f) {
        value = (int)(f * scale);  
    }
    
    public static implicit operator FloatF(float f) => new FloatF(f);

    public static FloatF operator+(FloatF a, FloatF b) => new FloatF(a.value + b.value);
    public static FloatF operator-(FloatF a, FloatF b) => new FloatF(a.value - b.value);
    public static FloatF operator*(FloatF a, FloatF b) => new FloatF(a.value * b.value / scale);
    public static FloatF operator/(FloatF a, FloatF b) => new FloatF(a.value * scale / b.value);
    
    public static bool operator>(FloatF a, FloatF b) => a.value > b.value;
    public static bool operator<(FloatF a, FloatF b) => a.value < b.value;
    public static bool operator>=(FloatF a, FloatF b) => a.value >= b.value;
    public static bool operator<=(FloatF a, FloatF b) => a.value <= b.value;
    public static bool operator==(FloatF a, FloatF b) => a.value == b.value;
    public static bool operator!=(FloatF a, FloatF b) => a.value != b.value;
    
    public override bool Equals(object obj) => obj is FloatF f && f.value == value;
    public override int GetHashCode() => value.GetHashCode();

    #region Json Converter
    private class FloatFConverter : JsonConverter<FloatF> {
        public override void WriteJson(JsonWriter writer, FloatF value, JsonSerializer serializer) {
            writer.WriteRawValue((1.0f * value.value / scale).ToString());
        }

        public override FloatF ReadJson(JsonReader reader, System.Type objectType, FloatF existingValue, bool hasExistingValue, JsonSerializer serializer) {
            FloatF ret = new FloatF();
            if (reader.Value is IConvertible) {
                ret.value = (int)(Convert.ToDouble(reader.Value) * scale);
            } else {
                throw new JsonSerializationException("FloatF Serialize failed");
            }
            return ret;
        }
    }
    
    #endregion
}


