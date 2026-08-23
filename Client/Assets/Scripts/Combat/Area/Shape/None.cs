using System;
using System.Collections.Generic;

namespace Combat.Area {
    [Serializable]
    public class None : Shape {
        public override List<int> Raycast(Vector3F position, Vector3F direction) {
            return new List<int>();
        }

        public override List<int> Raycast(int typeBitSet, Vector3F position, Vector3F direction) {
            return new List<int>();
        }
    }
}