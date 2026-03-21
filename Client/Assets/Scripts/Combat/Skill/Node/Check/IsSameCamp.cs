// 技能树判断节点：是否同阵营

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