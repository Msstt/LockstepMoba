using UnityEngine;

namespace Combat.Actor {
    public abstract class ActorCreator {
        public abstract Actor Create(GameObject go);

        public abstract string PrefabName { get; }

        protected int GetNewUid() {
            ActorSystem system = GameMgr.Instance.GetSystem<ActorSystem>();
            return system.GetUid();
        }
    }
}