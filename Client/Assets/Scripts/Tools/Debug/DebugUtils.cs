using Framework;
using UnityEngine;

public static class DebugUtils {
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default, float duration = 2f, float width = 1f) {
        if (color == default) {
            color = Color.red;
        }
        DebugMgr.Instance.DrawLine(start, end, color, duration, width);
    }
    
    public static void DrawLine(Vector3F start, Vector3F end, Color color = default, float duration = 2f, float width = 1f) {
        DrawLine(start.ToVector3(), end.ToVector3(), color, duration, width);
    }
    
    public static void DrawDot(Vector3 point, Color color = default, float duration = 2f, float size = 2f) {
        if (color == default) {
            color = Color.red;
        }
        DebugMgr.Instance.DrawDot(point, color, duration, size);
    }
    
    public static void DrawDot(Vector3F point, Color color = default, float duration = 2f, float size = 2f) {
        DrawDot(point.ToVector3(), color, duration, size);
    }
    
    public static void DrawCircle(Vector3 center, float radius, float width = 1f, int seg = 32) {
        Vector3 prev = center + Vector3.forward * radius;
        for (int i = 1; i <= seg; i++) {
            float angle = i * Mathf.PI * 2 / seg;
            Vector3 next = center + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;
            DrawLine(prev, next, Color.red, 2, width);
            prev = next;
        }
    }
}
