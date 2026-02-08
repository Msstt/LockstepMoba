using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class IsInRange : ParamSelectNode<IsInRange.Param> {
        public class Param {
            [LabelText("距离")]
            public FloatF Distance;
        }
        
        public IsInRange(JToken json) : base(json) { }
        
        public override int Select(Context context) {
            if (!context.Param.UidIsValid) {
                return InValidIndex;
            }

            Actor.Actor actor = ActorUtils.GetActor(context.ActorUid);
            Actor.Actor target = ActorUtils.GetActor(context.Param.Uid);
            if (actor == null || target == null) {
                return InValidIndex;
            }
            
            return Vector3F.Distance(actor.Pos, target.Pos) <= param.Distance ? 1 : 2;
        }
    }
}