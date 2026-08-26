using UnityEngine;
using GlobalConfig = Config;

namespace Combat.Actor {
    public class Champion : Actor {
        public Champion(int id, int uid, GameObject go, CampType camp) : base(id, uid, go, camp) {
            Type = ActorType.Champion;
            Config = GlobalConfig.Champion[id];
        }
        
        public override void BindCom() {
            AddComponent<MoveCom>();
            AddComponent<AnimCom>();
            AddComponent<SlotCom>();
            AddComponent<ControlCom>();
            
            AddComponent<LevelCom>();
            AddComponent<SkillCom>();
            AddComponent<BuffCom>();
            AddComponent<EquipmentCom>();
            
            AddComponent<VisionCom>();
            
            AddComponent<CommonUICom>();
        }
    }
}