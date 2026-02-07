using Combat.Actor;

namespace Combat.Skill.SkillNode {
    public class SetCD : Node {
        protected override NodeState OnEnter(Context context) {
            SkillCom com = GetCom<SkillCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            
            com.StartCD(context.TreeId, GetLevelNumber(context, Config.Skill[context.TreeId].CD));
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}