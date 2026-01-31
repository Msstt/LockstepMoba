using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class DamageToSingle : Node {
        [System.Serializable]
        public class Param {
            [LabelText("物理伤害")]
            public StatScaler Physical;
        }
        private Param param;

        public DamageToSingle(JToken json) {
            param = ParseParam<Param>(json);
        }
        
        public override NodeState OnEnter(Context context) {
            if (!context.Param.UidIsValid) {
                return NodeState.Fail;
            }
            Stats stats = GetStats(context);
            if (stats == null) {
                return NodeState.Fail;
            }
            Actor.Actor actor = ActorUtils.GetActor(context.Param.Uid);
            if (actor == null) {
                return NodeState.Fail;
            }

            HitInfo hitInfo = new HitInfo {
                attacker = context.Param.Uid,
                damage = new Damage {
                    physical = StatsUtils.GetValue(stats, param.Physical),
                }
            };
            actor.OnHit(hitInfo);
            return NodeState.Finish;
        }
        
        public override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}