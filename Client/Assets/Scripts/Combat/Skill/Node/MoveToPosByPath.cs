// 技能树节点：按路径移动到指定位置

using Combat.Actor;

namespace Combat.Skill.SkillNode {
    public class MoveToPosByPath : Node {
        public override NodeState OnEnter(Context context) {
            MoveCom com = GetCom<MoveCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            SetValue(context, "Res", -1);
            com.MoveToPosByPath(context.Param.Pos, () => {
                SetValue(context, "Res", 0);
            }, () => {
                SetValue(context, "Res", 1);
            });
            return NodeState.Continue;
        }

        public override NodeState OnUpdate(Context context) {
            int res = GetValueOrDefault<int>(context, "Res", -1);
            switch (res) {
                case -1:
                    return NodeState.Continue;
                case 0:
                    return NodeState.Finish;
                case 1:
                    return NodeState.Fail;
                default:
                    return NodeState.NoKnow;
            }
        }

        public override void OnFail(Context context) {
            GetCom<MoveCom>(context)?.ForceFail();
        }
    }
}