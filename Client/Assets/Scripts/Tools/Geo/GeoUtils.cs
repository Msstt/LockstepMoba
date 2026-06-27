using System;
using System.Collections.Generic;

// (XZ 平面)
public class GeoUtils {
    // 线段是否相交
    public static bool LineIsIntersect(Vector3F a, Vector3F b, Vector3F c, Vector3F d) {
        if (FloatF.Max(c.x, d.x) < FloatF.Min(a.x, b.x)) return false;
        if (FloatF.Max(c.z, d.z) < FloatF.Min(a.z, b.z)) return false;
        if (FloatF.Max(a.x, b.x) < FloatF.Min(c.x, d.x)) return false;
        if (FloatF.Max(a.z, b.z) < FloatF.Min(c.z, d.z)) return false;
        a.y = b.y = c.y = d.y = 0;
        if (Vector3F.Cross(c - a, d - a).y * Vector3F.Cross(c - b, d - b).y > 0) return false;
        if (Vector3F.Cross(a - c, b - c).y * Vector3F.Cross(a - d, b - d).y > 0) return false;
        return true;
    }
    
    // 线段交点 TODO：平行还没处理
    public static bool LineIntersect(Vector3F a, Vector3F b, Vector3F c, Vector3F d, out Vector3F intersection) {
        intersection = a;
        if (!LineIsIntersect(a, b, c, d)) {
            return false;
        }
        
        Vector3F ab = b - a, ac = c - a, ad = d - a;
        FloatF s1 = FloatF.Abs(Vector3F.Cross(ab, ac).y);
        FloatF s2 = FloatF.Abs(Vector3F.Cross(ab, ad).y);
        if (s2 < FloatF.eps) {
            return true;
        }
        FloatF t = s1 / s2;
        intersection = c + (d - c) * (t / (1 + t));
        return true;
    }
    
    // 点到线段的距离
    public static FloatF PointToSegment(Vector3F p, Vector3F a, Vector3F b) {
        p.y = a.y = b.y = 0;
        if (Vector3F.IsEqualInEps(a, b, FloatF.eps)) {
            return Vector3F.Distance(p, a);
        }
        Vector3F ab = b - a, ap = p - a;
        FloatF abLen = Vector3F.Distance2(a, b);
        FloatF t = Vector3F.Dot(ap, ab) / abLen;
        if (t < 0) {
            t = 0;
        } else if (t > 1) {
            t = 1;
        }
        return Vector3F.Distance(p, a + ab * t);
    }
    
    // 点是否在多边形内
    public static bool PointInPolygon(Vector3F p, List<Vector3F> polygon) {
        int n = polygon.Count;
        if (n < 3) return false;
        FloatF E6 = new FloatF(1000_000);
        int count = 0;
        for (int i = 0; i < n; i++) {
            Vector3F a = polygon[i];
            Vector3F b = polygon[(i + 1) % n];
            if (LineIsIntersect(a, b, p, new Vector3F(p.x + E6, p.y, p.z))) {
                count++;
            }
        }
        return count % 2 == 1;
    }
}
