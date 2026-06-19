using Combat.Actor;
using Newtonsoft.Json.Linq;

namespace Combat.Buff.Effect {
    public abstract class ChangeStats<Param> : Effect<Param> {
        public ChangeStats(Buff buff, JToken json) : base(buff, json) { }

        public sealed override void OnCreate() {
            Actor.Actor actor = ActorUtils.GetActor(buff.ActorId);
            if (actor != null) {
                Add(actor.Stats);
            }
        }
        
        public sealed override void OnRefresh() {
            Actor.Actor actor = ActorUtils.GetActor(buff.ActorId);
            if (actor != null) {
                Remove(actor.Stats);
                Add(actor.Stats);
            }
        }
        
        public sealed override void OnUpdate() { }

        public sealed override void OnDestroy() {
            Actor.Actor actor = ActorUtils.GetActor(buff.ActorId);
            if (actor != null) {
                Remove(actor.Stats);
            }
        }

        protected abstract void Add(Stats stats);
        protected abstract void Remove(Stats stats);
    }
}