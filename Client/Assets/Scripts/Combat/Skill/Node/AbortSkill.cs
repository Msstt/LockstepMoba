using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class AbortSkill : Node {
        [System.Serializable]
        public class Param {
            [DrawWithUnity]
            [LabelText("技能类型")]
            public SkillType SkillList;
        }
        private Param param;
        
        public AbortSkill(JToken json) {
            param = ParseParam<Param>(json);
        }
        
        protected override NodeState OnEnter(Context context) {
            SkillCom com = GetCom<SkillCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            
            com.AbortSkill(param.SkillList, context.TreeId);
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}