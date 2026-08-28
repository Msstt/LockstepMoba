using System;

namespace Framework {
    public static class StatusCode {
        private const int Prime = 16777619;
        public const int Seed = unchecked((int)2166136261);

        public static int Combine(int code, bool value) => Combine(code, value ? 1 : 0);

        public static int Combine(int code, int value) {
            return unchecked((code ^ value) * Prime);
        }

        public static int Combine(int code, long value) {
            code = Combine(code, unchecked((int)value));
            return Combine(code, unchecked((int)(value >> 32)));
        }

        public static int Combine(int code, FloatF value) => Combine(code, value.RawValue);

        public static int Combine(int code, Vector3F value) {
            code = Combine(code, value.x);
            code = Combine(code, value.y);
            return Combine(code, value.z);
        }

        public static int Combine(int code, string value) {
            if (value == null) {
                return Combine(code, -1);
            }

            code = Combine(code, value.Length);
            foreach (char c in value) {
                code = Combine(code, c);
            }
            return code;
        }

        public static int CombineType(int code, Type type) => Combine(code, type?.FullName);

        public static int CombineData(int code, ICheckableData value) {
            if (value == null) {
                return Combine(code, 0);
            }
            code = Combine(code, 1);
            code = CombineType(code, value.GetType());
            return Combine(code, value.GetStatusCode());
        }

        public static int CombineValue(int code, object value) {
            if (value == null) {
                return Combine(code, 0);
            }

            code = Combine(code, 1);
            code = CombineType(code, value.GetType());
            switch (value) {
                case bool boolValue:
                    return Combine(code, boolValue);
                case byte byteValue:
                    return Combine(code, byteValue);
                case sbyte sbyteValue:
                    return Combine(code, sbyteValue);
                case short shortValue:
                    return Combine(code, shortValue);
                case ushort ushortValue:
                    return Combine(code, ushortValue);
                case int intValue:
                    return Combine(code, intValue);
                case uint uintValue:
                    return Combine(code, unchecked((int)uintValue));
                case long longValue:
                    return Combine(code, longValue);
                case ulong ulongValue:
                    return Combine(code, unchecked((long)ulongValue));
                case string stringValue:
                    return Combine(code, stringValue);
                case FloatF fixedValue:
                    return Combine(code, fixedValue);
                case Vector3F vectorValue:
                    return Combine(code, vectorValue);
                case Enum enumValue:
                    return Combine(code, Convert.ToInt64(enumValue));
                case ICheckableData checkable:
                    return Combine(code, checkable.GetStatusCode());
                default:
                    // 回调令牌等引用只记录“存在及类型”，不使用进程相关的对象哈希。
                    return code;
            }
        }
    }
}
