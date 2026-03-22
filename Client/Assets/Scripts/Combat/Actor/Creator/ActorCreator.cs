using UnityEngine;

namespace Combat.Actor {
    public abstract class ActorCreator {
        public abstract Actor Create(GameObject go);

        public abstract string PrefabName { get; }
    }
}