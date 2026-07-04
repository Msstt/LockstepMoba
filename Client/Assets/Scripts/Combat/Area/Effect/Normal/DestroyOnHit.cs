using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Area.Effect {
    public class DestroyOnHit : Effect<DestroyOnHit.Param> {
        public class Param {
        }
        
        public DestroyOnHit(Area area, int raycastId, JToken json) : base(area, raycastId, json) {}

        private int nextFrame;
        private HitInfo hitInfo;

        public override void OnUpdate() {
            bool has = false;
            Raycast((actor) => {
                has = true;
            });
            if (has) {
                AreaUtils.DestroyArea(area.Uid);
            }
        }
    }
}