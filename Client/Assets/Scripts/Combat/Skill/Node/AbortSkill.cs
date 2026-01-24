using Combat.Actor;
using Newtonsoft.Json.Linq;

namespace Combat.Skill.SkillNode {
    public class AbortSKillParam {
        public SkillType SkillList;
    }
    
    public class AbortSkill : Node {
        private SkillType skillList;
        
        public AbortSkill(JToken json) {
            AbortSKillParam param = ParseParam<AbortSKillParam>(json);
            skillList = param.SkillList;
        }
        
        public override NodeState OnEnter(Context context) {
            SkillCom com = GetCom<SkillCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            com.AbortSkill(skillList, context.TreeId);
            return NodeState.Finish;
        }
        
        public override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}