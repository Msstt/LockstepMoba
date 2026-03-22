using UnityEngine;

namespace Combat.Actor {
    public interface IActorSystem : IInitSystem, IStartSystem, IUpdateSystem, IFrameUpdateSystem {
        public Transform TransRoot { get; }
        
        public int GetUid();
        public Actor GetActor(int uid);
        
        public Actor CreateActor(ActorCreator creator);
        public void RemoveActor(int uid);
        
        public bool IsSameCamp(int aUid, int bUid);
    }
}