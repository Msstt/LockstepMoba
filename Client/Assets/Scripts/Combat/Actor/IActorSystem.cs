using UnityEngine;

namespace Combat.Actor {
    public interface IActorSystem : ISystem {
        public Transform TransRoot { get; }
        
        public int GetUid();
    }
}