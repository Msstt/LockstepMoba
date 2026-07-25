// 技能树节点：打断指定类型的技能

using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class AbortSkill : ParamNode<AbortSkill.Param> {
        public class Param {
            [LabelText("技能类型")]
            public SkillType SkillList;
        }
        
        public AbortSkill(JToken json) : base(json) { }
        
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