using Combat.Skill;

namespace Combat.Actor {
    public class SkillCom : Com {
        private ISkillSystem system;
        
        private int[] skillId = null;
        
        public override void Awake() {
            system = GameMgr.Instance.GetSystem<ISkillSystem>();
            if (system == null) {
                throw new CombatException("SkillSystem is null");
            }
        }

        public void SetSkillId(ChampionConfig config) {
            skillId = new int[SkillUtils.SkillSlotCount];
            for (int i = 0; i < SkillUtils.SkillSlotCount; i++) {
                skillId[i] = config.skillIds[i];
            }
        }

        public void ExecuteSkill(SkillSlot slot, SkillParam param) {
            if (slot < 0 || (int)slot >= SkillUtils.SkillSlotCount || skillId == null) {
                Log.Error("Invalid skill slot: " + slot);
                return;
            }
            system.Execute(Actor.Uid, skillId[(int)slot], param);
        }

        public void AbortSkill(SkillType typeList, int excludeSkillId) {
            system.Abort(Actor.Uid, typeList, excludeSkillId);
        }
    }
}