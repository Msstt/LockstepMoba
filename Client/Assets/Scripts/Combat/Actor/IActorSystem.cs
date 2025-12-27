using UnityEngine;

namespace Combat.Actor {
    public interface IActorSystem : IInitSystem, IStartSystem, IUpdateSystem, IFrameUpdateSystem {
        public Transform TransRoot { get; }
        public Champion SelfChampion { get; }
        
        public int GetUid();
    }
}