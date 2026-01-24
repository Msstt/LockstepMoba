using Combat.Actor;

namespace Combat.Skill.SkillNode {
    public abstract class MoveNode : Node {
        protected abstract NodeState Move(Context context, MoveCom com);
        
        public override NodeState OnEnter(Context context) {
            MoveCom com = GetCom<MoveCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            
            SetValue(context, "Res", -1);
            return Move(context, com);
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

        protected void MoveFinish(Context context) {
            SetValue(context, "Res", 0);
        }
        
        protected void MoveFail(Context context) {
            SetValue(context, "Res", 0);
        }
    }
}