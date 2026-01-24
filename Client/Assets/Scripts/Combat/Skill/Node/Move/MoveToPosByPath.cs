// 技能树节点：按路径移动到指定位置

using Combat.Actor;

namespace Combat.Skill.SkillNode {
    public class MoveToPosByPath : MoveNode {
        protected override NodeState Move(Context context, MoveCom com) {
            if (!context.Param.PosIsValid) {
                return NodeState.Fail;
            }
            
            com.MoveToPosByPath(context.Param.Pos, () => {
                MoveFinish(context);
            }, () => {
                MoveFail(context);
            });
            return NodeState.Continue;
        }
    }
}