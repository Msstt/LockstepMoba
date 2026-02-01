using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class SetCD : Node {
        public class Param {
            [LabelText("冷却时间")]
            public FloatF CD;
        }
        private readonly Param param;
        
        public SetCD(JToken json) {
            param = ParseParam<Param>(json);
        }
        
        protected override NodeState OnEnter(Context context) {
            SkillCom com = GetCom<SkillCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            
            com.StartCD(context.TreeId, param.CD);
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}