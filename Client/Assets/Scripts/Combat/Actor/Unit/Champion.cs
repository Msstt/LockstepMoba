using UnityEngine;

namespace Combat.Actor {
    public class Champion : Actor {
        public Champion(int id, int uid, GameObject go, CampType camp) : base(id, uid, go, camp) {
            Type = ActorType.Champion;
        }
        
        public override void BindCom() {
            AddComponent<MoveCom>();
            AddComponent<AnimCom>();
            AddComponent<SlotCom>();
            
            AddComponent<LevelCom>();
            AddComponent<SkillCom>();
            AddComponent<BuffCom>();
            AddComponent<EquipmentCom>();
            
            AddComponent<VisionCom>();
            
            AddComponent<NormalUICom>();
        }
    }
}