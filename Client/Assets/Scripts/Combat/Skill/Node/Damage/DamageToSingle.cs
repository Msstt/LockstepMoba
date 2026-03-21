// 技能树节点：对单个目标造成伤害

using Combat.Actor;
using Newtonsoft.Json.Linq;

namespace Combat.Skill.SkillNode {
    public class DamageToSingle : ParamNode<LevelNumber<DamageInfo>> {
        public DamageToSingle(JToken json) : base(json) { }
        
        protected override NodeState OnEnter(Context context) {
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
                    physical = StatsUtils.GetValue(stats, GetLevelNumber(context, param).Physical),
                    magic = StatsUtils.GetValue(stats, GetLevelNumber(context, param).Magic),
                    @true = StatsUtils.GetValue(stats, GetLevelNumber(context, param).True),
                }
            };
            actor.OnHit(hitInfo);
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
    }
}