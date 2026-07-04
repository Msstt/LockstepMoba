using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Area.Effect {
    public class Heal : Effect<Heal.Param> {
        public class Param {
            [LabelText("伤害")]
            public LevelNumber<StatScaler> Heal;
            [LabelText("触发间隔")]
            public FloatF Interval;
        }
        
        public Heal(Area area, int raycastId, JToken json) : base(area, raycastId, json) {}

        private FloatF heal;
        private int nextFrame;
        
        public override void OnCreate() {
            heal = FloatF.zero;
            Actor.Actor actor = ActorUtils.GetActor(area.ActorId);
            if (actor != null) {
                heal = StatsUtils.GetValue(actor.Stats, GetLevelNumber(param.Heal));
            }
            
            nextFrame = TimeUtils.GetFrame(param.Interval);
            TakeDamage();
        }

        public override void OnUpdate() {
            if (GameMgr.Instance.Frame >= nextFrame) {
                nextFrame = TimeUtils.GetFrame(param.Interval);
                TakeDamage();
            }
        }

        private void TakeDamage() {
            Raycast((actor) => {
                actor.OnHeal(heal);
            });
        }
    }
}