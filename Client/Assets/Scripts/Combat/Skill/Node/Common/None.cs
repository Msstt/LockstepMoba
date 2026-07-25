// 技能树节点：空节点

namespace Combat.Skill.SkillNode {
    public class None : Node {
        protected override NodeState OnEnter(Context context) => NodeState.Finish;
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}