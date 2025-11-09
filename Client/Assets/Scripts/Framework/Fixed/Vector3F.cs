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
    
    public static implicit operator Vector3F(Vector3 v) => new Vector3F(v.x, v.y, v.z);
    
    public static Vector3F operator+(Vector3F a, Vector3F b) => new Vector3F(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Vector3F operator-(Vector3F a, Vector3F b) => new Vector3F(a.x - b.x, a.y - b.y, a.z - b.z);
    
    public static bool operator==(Vector3F a, Vector3F b) => a.x == b.x && a.y == b.y && a.z == b.z;
    public static bool operator!=(Vector3F a, Vector3F b) => a.x != b.x || a.y != b.y || a.z != b.z;
    
    public override bool Equals(object obj) => obj is Vector3F v && v == this;
    public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;
}
