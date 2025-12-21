using UnityEngine;

namespace Combat.Actor {
    public interface IActorSystem : ISystem {
        public Transform TransRoot { get; }
        public Champion SelfChampion { get; }
        
        public int GetUid();
    }
}