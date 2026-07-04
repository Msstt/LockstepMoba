using System.Collections.Generic;
using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Area.Raycast {
    public class AllByType : Raycast<AllByType.Param> {
        public class Param {
            [LabelText("是否同一阵营")]
            public bool IsSameCamp;
            [LabelText("角色类型")]
            public ActorType Type;
        }
        
        public AllByType(Area area, JToken json) : base(area, json) {}
        
        public override List<Actor.Actor> Get() {
            List<Actor.Actor> result = new List<Actor.Actor>();
            List<int> actors = area.Shape.Raycast((int)param.Type, area.Position, area.Direction);
            for (int i = 0; i < actors.Count; i++) {
                if (ActorUtils.IsSameCamp(area.ActorId, actors[i]) != param.IsSameCamp) {
                    continue;
                }
                Actor.Actor actor = ActorUtils.GetActor(actors[i]);
                if (actor != null) {
                    result.Add(actor);
                }
            }
            return result;
        }
    }
}