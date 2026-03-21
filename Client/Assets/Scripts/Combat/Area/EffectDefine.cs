using Combat.Area.Effect;

namespace Combat.Area {
    public enum EffectType {
        Heal = 1,
    }
    
    public static class EffectFactory {
        public static IEffect CreateEffect(Area area, EffectConfig config) {
            switch (config.Type) {
                case EffectType.Heal:
                    return new Heal(area, config.Params);
                default:
                    throw new CombatException("Effect type doesn't exist: " + config.Type);
            }
        }
    }
}