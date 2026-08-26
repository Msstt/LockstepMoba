using UnityEngine;

namespace Combat.Actor {
    public abstract class ActorCreator {
        public abstract Actor Create(GameObject go);

        public abstract string PrefabName { get; }

        protected int GetNewUid() {
            IActorSystem system = GameMgr.Instance.GetSystem<IActorSystem>();
            return system.GetUid();
        }
        
        protected void SetStatusByConfig(Actor actor, ActorConfig config) {
            actor.Stats.Health = new LimitedPriority(config.health[1]);
            
            actor.Stats.Attack = new Priority(config.attack[1]);
            actor.Stats.AttackSpeed = new Priority(config.attackSpeed[1]);
            actor.Stats.AttackDistance = new Priority(config.attackDistance);
            
            actor.Stats.MoveSpeed = new Priority(config.moveSpeed);
        }
    }
}