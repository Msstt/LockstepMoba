// 技能树判断节点：是否此单位类型

using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class IsActorType : ParamSelectNode<IsActorType.Param> {
        public class Param {
            [LabelText("单位类型")]
            public ActorType Type;
        }
        
        public IsActorType(JToken json) : base(json) { }
        
        public override int Select(Context context) {
            if (!context.Param.UidIsValid) {
                return InValidIndex;
            }
            
            Actor.Actor actor = ActorUtils.GetActor(context.Param.Uid);
            if (actor == null) {
                return InValidIndex;
            }

            return ((int)actor.Type & (int)param.Type) != 0 ? 1 : 2;
        }
    }
}