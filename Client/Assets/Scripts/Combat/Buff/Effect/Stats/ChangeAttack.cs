using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Buff.Effect {
    public class ChangeAttack : ChangeStats<ChangeAttack.Param> {
        public class Param {
            [LabelText("修改方式")]
            public Priority.ModifierType Type;
            [LabelText("攻击力")]
            public LevelNumber<FloatF> Attack;
        }
        
        public ChangeAttack(Buff buff, JToken json) : base(buff, json) { }

        protected override void Add(Stats stats) {
            stats.Attack.AddModifier(param.Type, GetLevelNumber(param.Attack) * buff.Count);
        }

        protected override void Remove(Stats stats) {
            stats.Attack.RemoveModifier(param.Type, GetLevelNumber(param.Attack)* buff.Count);
        }
    }
}