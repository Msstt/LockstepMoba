// 技能树节点：根据输入参数设置角色朝向

using InputSystem;

namespace Combat.Skill.SkillNode {
    public class SetDirByInputParam : Node {
        protected override NodeState OnEnter(Context context) {
            Actor.Actor actor = GetActor(context);
            if (actor == null) {
                return NodeState.Fail;
            }
            
            switch (Config.Skill[context.TreeId].InputType) {
                case CommandType.SinglePos:
                    actor.SetDir(context.Param.Pos - actor.Pos);
                    break;
                case CommandType.SingleUnit:
                    Actor.Actor target = ActorUtils.GetActor(context.Param.Uid);
                    if (target == null) {
                        return NodeState.Fail;
                    }
                    actor.SetDir(target.Pos - actor.Pos);
                    break;
                case CommandType.SingleDir:
                    actor.SetDir(context.Param.Dir);
                    break;
            }

            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}