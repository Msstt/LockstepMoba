using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Area.Effect {
    public class TargetActorMove : MoveEffect<TargetActorMove.Param> {
        public class Param {
            [LabelText("移动速度")]
            public FloatF Velocity;
            [LabelText("旋转速度")]
            public FloatF RotateSpeed;
        }

        public TargetActorMove(Area area, int raycastId, JToken json) : base(area, raycastId, json) {
            if (param.Velocity <= 0) {
                param.Velocity = ActorUtils.GetActor(area.ActorId)?.Config.areaVelocity ?? 5;
            }
        }
        
        public override void OnUpdate() {
            Actor.Actor actor = ActorUtils.GetActor(area.TargetUid);
            if (actor == null) {
                // TODO 飞向尸体
                AreaUtils.DestroyArea(area.Uid);
                return;
            }

            if (Vector3F.Distance(area.Position, actor.Pos) <= param.Velocity * TimeUtils.DeltaTime) {
                area.Position = actor.Pos;
            } else {
                area.Position += (actor.Pos - area.Position).Normalized() * param.Velocity * TimeUtils.DeltaTime;
            }

            // TODO RotateSpeed
            area.Direction = (area.Position - actor.Pos).Normalized();
        }
    }
}