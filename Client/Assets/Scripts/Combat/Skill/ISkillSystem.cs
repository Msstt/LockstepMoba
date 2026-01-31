
namespace Combat.Skill {
    public interface ISkillSystem : ISystem, IFrameUpdateSystem {
        public void Execute(int actorUid, int skillId, SkillParam param);
        public void ExecuteAsync(int actorUid, int skillId, SkillParam param);
        public void Abort(int actorUid, int skillId);
        public void Abort(int actorUid, SkillType typeList);
        public void AbortAsync(int actorUid, SkillType typeList, int excludeSkillId);
        
        public SkillType GetSkillType(int actorUid, int skillId);
    }
}