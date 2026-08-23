// 技能树节点：创建区域飞向目标单位

using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class CreateAreaToTargetUid : ParamNode<CreateAreaToTargetUid.Param> {
        public class Param {
            [LabelText("区域")]
            public int AreaId;
            [LabelText("偏移")]
            public SimpleTransform Offset;
        }

        public CreateAreaToTargetUid(JToken json) : base(json) { }
        
        protected override NodeState OnEnter(Context context) {
            if (!context.Param.UidIsValid) {
                return NodeState.Fail;
            }
            
            var actor = GetActor(context);
            if (actor == null) {
                return NodeState.Fail;
            }

            // TODO 普通攻击的通用偏移
            Vector3F rightDir = new Vector3F(actor.Dir.z, actor.Dir.y, -actor.Dir.x);
            Vector3F pos = actor.Pos + actor.Dir * param.Offset.position.x + rightDir * param.Offset.position.z;
            pos.y += param.Offset.position.y;    
            Vector3F dir = actor.Dir;
            (dir.x, dir.z) = (dir.x * FloatF.Cos(param.Offset.direction) - dir.z * FloatF.Sin(param.Offset.direction),
                dir.x * FloatF.Sin(param.Offset.direction) + dir.z * FloatF.Cos(param.Offset.direction));
            AreaUtils.CreateArea(param.AreaId, context.ActorUid, context.Level, pos, dir, context.Param.Uid);
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}