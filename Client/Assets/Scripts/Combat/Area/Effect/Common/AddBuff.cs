using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Area.Effect {
    public class AddBuff : Effect<AddBuff.Param> {
        public class Param {
            [LabelText("Buff")]
            public int BuffId;
        }
        
        public AddBuff(Area area, int raycastId, JToken json) : base(area, raycastId, json) { }
        
        public override void OnUpdate() {
            Raycast((actor) => {
                BuffCom com = actor.GetComponent<BuffCom>();
                com?.AddBuff(param.BuffId, area.ActorId, area.Level);
            });
        }
    }
}