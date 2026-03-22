using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Combat.Area {
    [Serializable]
    public class Circle : Shape {
        [LabelText("半径")]
        public FloatF radius;
        
        public override List<int> Raycast(Vector3F position, Vector3F direction) {
            return NavmeshUtils.RaycastInCircle(position, radius);
        }

        public override List<int> Raycast(int typeBitSet, Vector3F position, Vector3F direction) {
            return NavmeshUtils.RaycastInCircle(typeBitSet, position, radius);
        }
    }
}