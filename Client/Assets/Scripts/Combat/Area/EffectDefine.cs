using Combat.Area.Effect;

namespace Combat.Area {
    public enum EffectType {
        Heal = 1,
        LinearMove = 2,
        DestroyOnHit = 3,
        Damage = 4,
        AddBuff = 5,
        TargetActorMove = 6,
    }
    
    public static class EffectFactory {
        public static IEffect CreateEffect(Area area, EffectConfig config) {
            switch (config.Type) {
                case EffectType.Heal:
                    return new Heal(area, config.RaycastId, config.Params);
                case EffectType.LinearMove:
                    return new LinearMove(area, config.RaycastId, config.Params);
                case EffectType.DestroyOnHit:
                    return new DestroyOnHit(area, config.RaycastId, config.Params);
                case EffectType.Damage:
                    return new Damage(area, config.RaycastId, config.Params);
                case EffectType.AddBuff:
                    return new AddBuff(area, config.RaycastId, config.Params);
                case EffectType.TargetActorMove:
                    return new TargetActorMove(area, config.RaycastId, config.Params);
                default:
                    throw new CombatException("Effect type doesn't exist: " + config.Type);
            }
        }
    }
}