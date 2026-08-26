using UnityEngine;
using GlobalConfig = Config;

namespace Combat.Actor {
    public class Turret : Actor {
        public Turret(int id, int uid, GameObject go, CampType camp) : base(id, uid, go, camp) {
            Type = ActorType.Turret;
            Config = GlobalConfig.Turret[id];
        }
        
        public override void BindCom() {
            // AddComponent<AnimCom>();
            AddComponent<ControlCom>();
            
            AddComponent<BehaviourMachineCom>();
            AddComponent<BuffCom>();
            
            AddComponent<VisionCom>();
            
            AddComponent<CommonUICom>();
        }
    }
}