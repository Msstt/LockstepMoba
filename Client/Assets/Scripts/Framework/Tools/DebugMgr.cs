using UnityEngine;

namespace Framework {
    public class DebugMgr : MonoSingleton<DebugMgr> {
        public void DrawLine(Vector3 start, Vector3 end, Color color = default, float duration = 10f, float width = 1f) {
            if (color == default) {
                color = Color.red;
            }
            var lineObj = new GameObject("Line");
            lineObj.transform.SetParent(transform);
            var lr = lineObj.AddComponent<LineRenderer>();
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = width;
            lr.endWidth = width;
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
    
            if (duration > 0)
                Destroy(lineObj, duration);
        }
    }
}
