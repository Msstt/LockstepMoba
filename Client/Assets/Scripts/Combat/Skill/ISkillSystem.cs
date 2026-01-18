
namespace Combat.Skill {
    public interface ISkillSystem : ISystem, IFrameUpdateSystem {
        public void Execute(int actorUid, int skillId, SkillParam param);
        public void Abort(int actorUid, int skillId);
        
        public SkillType GetSkillType(int actorUid, int skillId);
    }
}