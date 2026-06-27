using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Combat.Area {
    [Serializable]
    public class Rect : Shape {
        [LabelText("长")]
        public FloatF length;
        [LabelText("宽")]
        public FloatF width;
        
        public override List<int> Raycast(Vector3F position, Vector3F direction) {
            return NavmeshUtils.RaycastInRect(position, direction, length, width);
        }

        public override List<int> Raycast(int typeBitSet, Vector3F position, Vector3F direction) {
            return NavmeshUtils.RaycastInRect(typeBitSet, position, direction, length, width);
        }
    }
}
