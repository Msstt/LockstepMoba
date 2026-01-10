using UnityEngine;

namespace Combat.Actor {
    public interface IActorSystem : IInitSystem, IStartSystem, IUpdateSystem, IFrameUpdateSystem {
        public Transform TransRoot { get; }
        
        public int GetUid();
        public Actor GetActor(int uid);
        
        public void RemoveActor(int uid);
    }
}