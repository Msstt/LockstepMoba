using System.Collections.Generic;

namespace Combat.Area {
    public class Circle : Shape {
        public FloatF radius;
        
        public override List<int> Raycast(Vector3F position, Vector3F direction) {
            return NavmeshUtils.RaycastInCircle(position, radius);
        }

        public override List<int> Raycast(int type, Vector3F position, Vector3F direction) {
            return NavmeshUtils.RaycastInCircle(type, position, radius);
        }
    }
}