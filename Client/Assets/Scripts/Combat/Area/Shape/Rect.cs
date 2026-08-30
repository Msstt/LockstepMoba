using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Area {
    [Serializable]
    public class Rect : Shape {
        [LabelText("长")]
        public FloatF length;
        [LabelText("宽")]
        public FloatF width;
        
        public override void Raycast(Vector3F position, Vector3F direction, List<int> results) {
            NavmeshUtils.RaycastInRect(position, direction, length, width, results);
        }

        public override void Raycast(int typeBitSet, Vector3F position, Vector3F direction, List<int> results) {
            NavmeshUtils.RaycastInRect(typeBitSet, position, direction, length, width, results);
        }

        // TODO 优化显示
        public override void RenderDebug(Vector3F position, Vector3F direction) {
            Vector3F forward = direction.Normalized();
            Vector3F right = new Vector3F(forward.z, 0, -forward.x);
            Vector3F halfLength = forward * (length / 2);
            Vector3F halfWidth = right * (width / 2);
            Vector3F p1 = position + halfLength + halfWidth;
            Vector3F p2 = position + halfLength - halfWidth;
            Vector3F p3 = position - halfLength - halfWidth;
            Vector3F p4 = position - halfLength + halfWidth;
            
            DebugUtils.DrawLine(p1.ToVector3(), p2.ToVector3(), Color.green, 0.1f);
            DebugUtils.DrawLine(p2.ToVector3(), p3.ToVector3(), Color.green, 0.1f);
            DebugUtils.DrawLine(p3.ToVector3(), p4.ToVector3(), Color.green, 0.1f);
            DebugUtils.DrawLine(p4.ToVector3(), p1.ToVector3(), Color.green, 0.1f);
        }
    }
}
