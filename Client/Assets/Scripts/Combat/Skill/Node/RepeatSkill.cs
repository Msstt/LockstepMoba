using Combat.Actor;

namespace Combat.Skill.SkillNode {
    public class RepeatSkill : Node {
        protected override NodeState OnEnter(Context context) => NodeState.Finish;
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;

        protected override void OnFinish(Context context) {
            GetCom<SkillCom>(context)?.ExecuteSkillAsync(context.TreeId, context.Param);
        }
    }
}