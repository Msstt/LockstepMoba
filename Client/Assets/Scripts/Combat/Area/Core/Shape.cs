using System.Collections.Generic;

namespace Combat.Area {
    public abstract class Shape {
        public abstract List<int> Raycast(Vector3F position, Vector3F direction);
        public abstract List<int> Raycast(int typeList, Vector3F position, Vector3F direction);
        
        public virtual void RenderDebug(Vector3F position, Vector3F direction) { }
    }
}