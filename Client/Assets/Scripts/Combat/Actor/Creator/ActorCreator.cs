using UnityEngine;

namespace Combat.Actor {
    public abstract class ActorCreator {
        public abstract Actor Create(GameObject go);

        public abstract string PrefabName { get; }

        protected int GetNewUid() {
            IActorSystem system = GameMgr.Instance.GetSystem<IActorSystem>();
            return system.GetUid();
        }
    }
}