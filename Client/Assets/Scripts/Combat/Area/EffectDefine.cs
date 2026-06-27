using Combat.Area.Effect;

namespace Combat.Area {
    public enum EffectType {
        Heal = 1,
        LinearMove = 2,
    }
    
    public static class EffectFactory {
        public static IEffect CreateEffect(Area area, EffectConfig config) {
            switch (config.Type) {
                case EffectType.Heal:
                    return new Heal(area, config.Params);
                case EffectType.LinearMove:
                    return new LinearMove(area, config.Params);
                default:
                    throw new CombatException("Effect type doesn't exist: " + config.Type);
            }
        }
    }
}