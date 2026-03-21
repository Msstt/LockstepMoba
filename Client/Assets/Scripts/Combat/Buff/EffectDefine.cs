using Combat.Buff.Effect;

namespace Combat.Buff {
    public enum EffectType {
        Damage = 1,
    }
    
    public static class EffectFactory {
        public static IEffect CreateEffect(Buff buff, EffectConfig config) {
            switch (config.Type) {
                case EffectType.Damage:
                    return new Damage(buff, config.Params);
                default:
                    throw new CombatException("Buff Effect type doesn't exist: " + config.Type);
            }
        }
    }
}