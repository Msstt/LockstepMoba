using System.Collections.Generic;

namespace Combat.Area {
    public abstract class Shape {
        public abstract void Raycast(Vector3F position, Vector3F direction, List<int> results);
        public abstract void Raycast(int typeList, Vector3F position, Vector3F direction, List<int> results);
        
        public virtual void RenderDebug(Vector3F position, Vector3F direction) { }
    }
}
