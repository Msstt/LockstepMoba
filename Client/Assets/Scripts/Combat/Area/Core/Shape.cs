using System.Collections.Generic;

namespace Combat.Area {
    public abstract class Shape {
        public abstract List<int> Raycast(Vector3F position, Vector3F direction);
        public abstract List<int> Raycast(int type, Vector3F position, Vector3F direction);
    }
}