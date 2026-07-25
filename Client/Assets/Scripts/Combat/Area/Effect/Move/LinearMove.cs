using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Area.Effect {
    public class LinearMove : Effect<LinearMove.Param> {
        public class Param {
            [LabelText("移动速度")]
            public FloatF Velocity;
        }
        
        public LinearMove(Area area, int raycastId, JToken json) : base(area, raycastId, json) { }
        
        public override void OnUpdate() {
            area.Position += area.Direction * param.Velocity * TimeUtils.DeltaTime;
        }
    }
}