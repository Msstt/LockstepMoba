using Combat.Actor;
using Newtonsoft.Json.Linq;

namespace Combat.Skill.SkillNode {
    public class AbortSkill : Node {
        private class Param {
            public SkillType SkillList;
        }
        private Param param;
        
        public AbortSkill(JToken json) {
            param = ParseParam<Param>(json);
        }
        
        public override NodeState OnEnter(Context context) {
            SkillCom com = GetCom<SkillCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            
            com.AbortSkill(param.SkillList, context.TreeId);
            return NodeState.Finish;
        }
        
        public override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}