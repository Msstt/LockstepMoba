using UnityEngine;

namespace Combat.Actor {
    public interface IActorSystem : IInitSystem, IStartSystem, IUpdateSystem, IFrameUpdateSystem {
        public Transform TransRoot { get; }
        
        public int GetUid();
        public Actor GetChampion(Uid uid);
    }
}