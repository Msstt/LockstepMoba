// 技能树节点：在自身位置创建区域

using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class CreateAreaAtSelf : ParamNode<CreateAreaAtSelf.Param> {
        public class Param {
            [LabelText("区域")]
            public int AreaId;
            [LabelText("偏移")]
            public Vector3F Offset;
            [LabelText("方向")]
            public FloatF Direction;
        }

        public CreateAreaAtSelf(JToken json) : base(json) {
            param.Direction = param.Direction / 180 * FloatF.pi;
        }
        
        protected override NodeState OnEnter(Context context) {
            var actor = ActorUtils.GetActor(context.ActorUid);
            if (actor == null) {
                return NodeState.Fail;
            }

            Vector3F rightDir = new Vector3F(actor.Dir.z, actor.Dir.y, -actor.Dir.x);
            Vector3F pos = actor.Pos + actor.Dir * param.Offset.x + rightDir * param.Offset.z;
            pos.y += param.Offset.y;    
            Vector3F dir = actor.Dir;
            (dir.x, dir.z) = (dir.x * FloatF.Cos(param.Direction) - dir.z * FloatF.Sin(param.Direction),
                dir.x * FloatF.Sin(param.Direction) + dir.z * FloatF.Cos(param.Direction));
            AreaUtils.CreateArea(param.AreaId, context.ActorUid, context.Level, pos, dir);
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}