using UnityEngine;

namespace Combat.Actor {
    public class Minion : Actor {
        public Minion(int id, int uid, GameObject go, CampType camp) : base(id, uid, go, camp) {
            Type = ActorType.Minion;
        }
        
        public override void BindCom() {
            AddComponent<MoveCom>();
            AddComponent<AnimCom>();
            AddComponent<ControlCom>();
            
            AddComponent<BehaviourMachineCom>();
            AddComponent<BuffCom>();
            
            AddComponent<VisionCom>();
            
            AddComponent<NormalUICom>();
        }
    }
}