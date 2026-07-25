// 技能树节点：重复技能，目前只用于普通攻击这样的需要自循环的技能，其他慎用

using Combat.Actor;

namespace Combat.Skill.SkillNode {
    public class RepeatSkill : Node {
        protected override NodeState OnEnter(Context context) => NodeState.Finish;
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;

        protected override void OnFinish(Context context) {
            GetCom<SkillCom>(context)?.ExecuteSkillAsync(context.TreeId, context.Level, context.Param);
        }
    }
}