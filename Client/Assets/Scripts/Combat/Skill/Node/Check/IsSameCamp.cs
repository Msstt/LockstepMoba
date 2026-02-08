namespace Combat.Skill.SkillNode {
    public class IsSameCamp : SelectNode {
        public override int Select(Context context) {
            if (!context.Param.UidIsValid) {
                return InValidIndex;
            }
            
            return ActorUtils.IsSameCamp(context.ActorUid, context.Param.Uid) ? 1 : 2;
        }
    }
}