using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Combat.Area {
    [Serializable]
    public class Circle : Shape {
        [LabelText("半径")]
        public FloatF radius;
        
        public override void Raycast(Vector3F position, Vector3F direction, List<int> results) {
            NavmeshUtils.RaycastInCircle(position, radius, results);
        }

        public override void Raycast(int typeBitSet, Vector3F position, Vector3F direction, List<int> results) {
            NavmeshUtils.RaycastInCircle(typeBitSet, position, radius, results);
        }
    }
}
