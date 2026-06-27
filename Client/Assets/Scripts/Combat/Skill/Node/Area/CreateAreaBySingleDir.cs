// 技能树节点：根据单一方向创建区域

using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class CreateAreaBySingleDir : ParamNode<CreateAreaBySingleDir.Param> {
        public class Param {
            [LabelText("区域")]
            public int AreaId;
        }

        public CreateAreaBySingleDir(JToken json) : base(json) { }
        
        protected override NodeState OnEnter(Context context) {
            if (!context.Param.DirIsValid) {
                return NodeState.Fail;
            }
            var actor = GetActor(context);
            if (actor == null) {
                return NodeState.Fail;
            }

            AreaUtils.CreateArea(param.AreaId, context.ActorUid, context.Level, actor.Pos, context.Param.Dir);
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}