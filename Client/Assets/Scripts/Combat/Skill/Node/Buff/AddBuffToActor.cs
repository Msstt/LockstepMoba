// 技能树节点：给目标添加Buff

using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class AddBuffToActor : ParamNode<AddBuffToActor.Param> {
        public class Param {
            [LabelText("Buff")]
            public int BuffId;
        }
        
        public AddBuffToActor(JToken json) : base(json) { }
        
        protected override NodeState OnEnter(Context context) {
            if (!context.Param.UidIsValid) {
                return NodeState.Fail;
            }
            BuffCom com = ActorUtils.GetCom<BuffCom>(context.Param.Uid);
            if (com == null) {
                return NodeState.Fail;
            }
            com.AddBuff(param.BuffId, context.ActorUid, context.Level);
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}