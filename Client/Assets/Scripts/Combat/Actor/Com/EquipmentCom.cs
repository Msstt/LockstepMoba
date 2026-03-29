using System.Collections.Generic;
using Combat.Equipment;
using Combat.Skill;

namespace Combat.Actor {
    public class EquipmentCom : PersistentCom {
        private static readonly int InvalidEquipmentId = 0;
        private static readonly Dictionary<int, SkillSlot> Slot = new Dictionary<int, SkillSlot>() {
            { 1, SkillSlot.Equipment1 },
            { 2, SkillSlot.Equipment2 },
            { 3, SkillSlot.Equipment3 },
            { 4, SkillSlot.Equipment4 },
            { 5, SkillSlot.Equipment5 },
            { 6, SkillSlot.Equipment6 },
        };
        
        private List<int> equipmentList = new List<int>(6);

        public void AddEquipment(int equipmentId) {
            for (int i = 0; i < equipmentList.Count; i++) {
                if (equipmentList[i] == InvalidEquipmentId) {
                    SetEquipmentId(i, equipmentId);
                    break;
                }
            }
        }
        
        public void RemoveEquipment(int index) {
            SetEquipmentId(index, InvalidEquipmentId);
        }

        private void SetEquipmentId(int index, int equipmentId) {
            EquipmentConfig config;
            BuffCom buffCom = ActorUtils.GetCom<BuffCom>(Uid);
            if (equipmentList[index] != InvalidEquipmentId) {
                config = Config.Equipment[equipmentList[index]];
                foreach (var buffId in config.buffId) {
                    buffCom?.RemoveBuff(buffId, Uid);
                }
            }
            equipmentList[index] = equipmentId;
            config = Config.Equipment[equipmentList[index]];
            foreach (var buffId in config.buffId) {
                buffCom?.AddBuff(buffId, Uid, 1);
            }
            SkillCom skillCom = ActorUtils.GetCom<SkillCom>(Uid);
            skillCom?.ChangeSkill(Slot[index], config.skillId);
        }
    }
}