using Newtonsoft.Json.Linq;

namespace Combat.Skill.SkillNode {
    public class TeleportToPos : Node {
        public class Param {
            public FloatF MaxDistance;
        }
        private Param param;
        
        public TeleportToPos(JToken json) {
            param = ParseParam<Param>(json);
        }
        
        protected override NodeState OnEnter(Context context) {
            if (!context.Param.PosIsValid) {
                return NodeState.Fail;
            }
            var actor = ActorUtils.GetActor(context.ActorUid);
            if (actor == null) {
                return NodeState.Fail;
            }

            Vector3F target = context.Param.Pos;
            if (Vector3F.Distance(actor.Pos, target) > param.MaxDistance) {
                Vector3F dir = (target - actor.Pos).Normalized();
                target = actor.Pos + dir * param.MaxDistance;
            }

            target = NavmeshUtils.RaycastInSurface(actor.Pos, target);
            DebugUtils.DrawDot(target);
            
            actor.SetPos(target, true);

            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}