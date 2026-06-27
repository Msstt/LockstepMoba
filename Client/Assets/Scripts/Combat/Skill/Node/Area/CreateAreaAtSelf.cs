// 技能树节点：在自身位置创建区域

using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class CreateAreaAtSelf : ParamNode<CreateAreaAtSelf.Param> {
        public class Param {
            [LabelText("区域")]
            public int AreaId;
            [LabelText("偏移")]
            public SimpleTransform Offset;
        }

        public CreateAreaAtSelf(JToken json) : base(json) { }
        
        protected override NodeState OnEnter(Context context) {
            var actor = GetActor(context);
            if (actor == null) {
                return NodeState.Fail;
            }

            Vector3F rightDir = new Vector3F(actor.Dir.z, actor.Dir.y, -actor.Dir.x);
            Vector3F pos = actor.Pos + actor.Dir * param.Offset.position.x + rightDir * param.Offset.position.z;
            pos.y += param.Offset.position.y;    
            Vector3F dir = actor.Dir;
            (dir.x, dir.z) = (dir.x * FloatF.Cos(param.Offset.direction) - dir.z * FloatF.Sin(param.Offset.direction),
                dir.x * FloatF.Sin(param.Offset.direction) + dir.z * FloatF.Cos(param.Offset.direction));
            AreaUtils.CreateArea(param.AreaId, context.ActorUid, context.Level, pos, dir);
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}