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
        
        public Heal(Area area, JToken json) : base(area, json) {}

        private FloatF heal;
        private int nextFrame;
        
        public override void OnCreate() {
            heal = FloatF.zero;
            Actor.Actor actor = ActorUtils.GetActor(area.ActorId);
            if (actor != null) {
                heal = StatsUtils.GetValue(actor.Stats, GetLevelNumber(param.Heal));
            }
            
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
                // if (ActorUtils.IsSameCamp(area.ActorId, actor.Uid)) {
                    actor.OnHeal(heal);
                // }
            });
        }
    }
}