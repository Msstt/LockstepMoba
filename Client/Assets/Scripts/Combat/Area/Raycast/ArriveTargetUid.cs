using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Combat.Area.Raycast {
    public class ArriveTargetUid : Raycast<ArriveTargetUid.Param> {
        public class Param {
        }
        
        public ArriveTargetUid(Area area, JToken json) : base(area, json) {}
        
        public override List<Actor.Actor> Get() {
            Actor.Actor actor = ActorUtils.GetActor(area.TargetUid);
            if (actor != null && area.Position == actor.Pos) {
                return new List<Actor.Actor> { actor };
            }
            return new List<Actor.Actor>();
        }
    }
}