using Framework;
using UnityEngine;

public static class DebugUtils {
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default, float duration = 2f, float width = 0.05f) {
        if (color == default) {
            color = Color.red;
        }
        DebugMgr.Instance.DrawLine(start, end, color, duration, width);
    }
    
    public static void DrawLine(Vector3F start, Vector3F end, Color color = default, float duration = 2f, float width = 0.05f) {
        DrawLine(start.ToVector3(), end.ToVector3(), color, duration, width);
    }
    
    public static void DrawDot(Vector3 point, Color color = default, float duration = 2f, float size = 0.1f) {
        if (color == default) {
            color = Color.red;
        }
        DebugMgr.Instance.DrawDot(point, color, duration, size);
    }
    
    public static void DrawDot(Vector3F point, Color color = default, float duration = 2f, float size = 0.1f) {
        DrawDot(point.ToVector3(), color, duration, size);
    }
}
