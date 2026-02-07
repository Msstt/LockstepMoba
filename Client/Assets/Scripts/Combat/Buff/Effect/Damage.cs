using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Buff.Effect {
    public class Damage : Effect<Damage.Param> {
        public class Param {
            [LabelText("伤害")]
            public DamageInfo Damage;
            [LabelText("触发间隔")]
            public FloatF Interval;
        }
        
        public Damage(Buff buff, JToken json) : base(buff, json) {}
    }
}