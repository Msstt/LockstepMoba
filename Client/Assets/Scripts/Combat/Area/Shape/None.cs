using System;
using System.Collections.Generic;

namespace Combat.Area {
    [Serializable]
    public class None : Shape {
        public override void Raycast(Vector3F position, Vector3F direction, List<int> results) {
        }

        public override void Raycast(int typeBitSet, Vector3F position, Vector3F direction, List<int> results) {
        }
    }
}
