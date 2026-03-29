using Combat.Buff.Effect;

namespace Combat.Buff {
    public enum EffectType {
        Damage = 1,        ChangeAttack = 2,
    }
    
    public static class EffectFactory {
        public static IEffect CreateEffect(Buff buff, EffectConfig config) {
            switch (config.Type) {
                case EffectType.Damage:
                    return new Damage(buff, config.Params);                case EffectType.ChangeAttack:
                    return new ChangeAttack(buff, config.Params);
                default:
                    throw new CombatException("Effect type doesn't exist: " + config.Type);
            }
        }
    }
}