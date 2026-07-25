using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Area.Effect {
    public class Damage : Effect<Damage.Param> {
        public class Param {
            [LabelText("伤害")]
            public LevelNumber<DamageInfo> Damage;
            [LabelText("触发间隔")]
            public FloatF Interval;
        }
        
        public Damage(Area area, int raycastId, JToken json) : base(area, raycastId, json) { }

        private int nextFrame;
        private HitInfo hitInfo;
        
        public override void OnCreate() {
            Actor.Damage damage = Actor.Damage.zero;
            Actor.Actor actor = ActorUtils.GetActor(area.ActorId);
            if (actor != null) {
                damage = new Actor.Damage {
                    physical = StatsUtils.GetValue(actor.Stats, GetLevelNumber(param.Damage).Physical),
                    magic = StatsUtils.GetValue(actor.Stats, GetLevelNumber(param.Damage).Magic),
                    @true = StatsUtils.GetValue(actor.Stats, GetLevelNumber(param.Damage).True),
                };
            }
            hitInfo = new HitInfo {
                attacker = area.ActorId,
                damage = damage,
            };
            
            nextFrame = TimeUtils.GetFrame(param.Interval);
            TakeHeal();
        }

        public override void OnUpdate() {
            if (GameMgr.Instance.Frame >= nextFrame) {
                nextFrame = TimeUtils.GetFrame(param.Interval);
                TakeHeal();
            }
        }

        private void TakeHeal() {
            Raycast((actor) => {
                actor.OnHit(hitInfo);
            });
        }
    }
}