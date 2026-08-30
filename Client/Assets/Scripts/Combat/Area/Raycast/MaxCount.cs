using System.Collections.Generic;
using Framework;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Area.Raycast {
    public class MaxCount : Raycast<MaxCount.Param> {
        public class Param {
            [LabelText("是否同一阵营")]
            public bool IsSameCamp;
            [LabelText("数量")]
            public int Count;
        }
        
        public MaxCount(Area area, JToken json) : base(area, json) {}
        
        public override List<Actor.Actor> Get() {
            List<Actor.Actor> result = new List<Actor.Actor>();
            using (PooledList<int> actors = PooledList<int>.Get()) {
                area.Shape.Raycast(area.Position, area.Direction, actors);
                for (int i = 0; i < actors.Count; i++) {
                    if (ActorUtils.IsSameCamp(area.ActorId, actors[i]) != param.IsSameCamp) {
                        continue;
                    }
                    Actor.Actor actor = ActorUtils.GetActor(actors[i]);
                    if (actor != null) {
                        result.Add(actor);
                        if (result.Count >= param.Count) {
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
