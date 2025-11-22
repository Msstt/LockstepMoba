using System;

public class GeoUtils {
    // 线段 XZ 平面相交
    public static bool LineIsIntersectInXZ(Vector3F a, Vector3F b, Vector3F c, Vector3F d) {
        if (FloatF.Max(c.x, d.x) < FloatF.Min(a.x, b.x)) return false;
        if (FloatF.Max(c.z, d.z) < FloatF.Min(a.z, b.z)) return false;
        if (FloatF.Max(a.x, b.x) < FloatF.Min(c.x, d.x)) return false;
        if (FloatF.Max(a.z, b.z) < FloatF.Min(c.z, d.z)) return false;
        a.y = b.y = c.y = d.y = 0;
        if (Vector3F.Cross(c - a, d - a).y * Vector3F.Cross(c - b, d - b).y > 0) return false;
        if (Vector3F.Cross(a - c, b - c).y * Vector3F.Cross(a - d, b - d).y > 0) return false;
        return true;
    }
}
