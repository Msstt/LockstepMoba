
namespace Combat.Skill {
    public interface ISkillSystem : ISystem, IFrameUpdateSystem {
        public void Execute(int actorUid, int skillId, int level, SkillParam param);
        public void Abort(int actorUid, int skillId);
        public void Abort(int actorUid, SkillType typeList);
        
        // 技能树节点中请用异步版本
        public void ExecuteAsync(int actorUid, int skillId, int level, SkillParam param);
        public void AbortAsync(int actorUid, SkillType typeList, int excludeSkillId);
        
        public SkillType GetSkillType(int actorUid, int skillId);
    }
}