using Newtonsoft.Json.Linq;

namespace Combat.Buff.Effect {
    public class Invisibility : Effect<Invisibility.Param> {
        public class Param {
        }
        
        public Invisibility(Buff buff, JToken json) : base(buff, json) { }

        public override void OnCreate() {
            Actor.Actor actor = ActorUtils.GetActor(buff.ActorId);
            if (actor != null) {
                actor.Stats.Invisibility += 1;
            }
        }

        public override void OnDestroy() {
            Actor.Actor actor = ActorUtils.GetActor(buff.ActorId);
            if (actor != null) {
                actor.Stats.Invisibility -= 1;
            }
        }
    }
}