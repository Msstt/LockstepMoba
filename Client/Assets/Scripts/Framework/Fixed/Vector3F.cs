// 帧同步 Vector3

using UnityEngine;

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
    
    public static Vector3F operator+(Vector3F a, Vector3F b) => new Vector3F(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Vector3F operator-(Vector3F a, Vector3F b) => new Vector3F(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Vector3F operator*(Vector3F a, FloatF x) => new Vector3F(a.x * x, a.y * x, a.z * x);
    public static Vector3F operator/(Vector3F a, FloatF x) => new Vector3F(a.x / x, a.y / x, a.z / x);
    
    public static bool operator==(Vector3F a, Vector3F b) => a.x == b.x && a.y == b.y && a.z == b.z;
    public static bool operator!=(Vector3F a, Vector3F b) => a.x != b.x || a.y != b.y || a.z != b.z;
    
    public override bool Equals(object obj) => obj is Vector3F v && v == this;
    public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;

    public static FloatF DistanceF(Vector3F a, Vector3F b) {
        FloatF dx = a.x - b.x;
        FloatF dy = a.y - b.y;
        FloatF dz = a.z - b.z;
        return dx * dx + dy * dy + dz * dz;
    }
}
