using Framework;
using UnityEngine;

public static class DebugUtils {
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default, float duration = 10f, float width = 1f) {
        DebugMgr.Instance.DrawLine(start, end, color, duration, width);
    }
}
