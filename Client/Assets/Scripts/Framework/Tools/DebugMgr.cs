using UnityEngine;

namespace Framework {
    public class DebugMgr : MonoSingleton<DebugMgr> {
        public void DrawLine(Vector3 start, Vector3 end, Color color, float duration, float width) {
            var lineObj = new GameObject("Line");
            lineObj.transform.SetParent(transform);
            var line = lineObj.AddComponent<LineRenderer>();
            line.startColor = color;
            line.endColor = color;
            line.startWidth = width;
            line.endWidth = width;
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            line.material = new Material(Shader.Find("Sprites/Default"));

            if (duration > 0) {
                Destroy(lineObj, duration);
            }
        }
        
        public void DrawDot(Vector3 point, Color color, float duration, float size) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(transform);
            sphere.transform.localScale = Vector3.one * size;
            sphere.transform.position = point;
            var renderer = sphere.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = color;
            Destroy(sphere.GetComponent<Collider>());
            if (duration > 0) {
                Destroy(sphere, duration);
            }
        }
    }
}
