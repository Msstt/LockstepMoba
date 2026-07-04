using Combat.Area.Raycast;

namespace Combat.Area {
    public enum RaycastType {
        All = 1,
        AllByType = 2,
        MaxCount = 3,
    }
    
    public static class RaycastFactory {
        public static IRaycast CreateEffect(Area area, RaycastConfig config) {
            switch (config.Type) {
                case RaycastType.All:
                    return new All(area, config.Params);
                case RaycastType.AllByType:
                    return new AllByType(area, config.Params);
                case RaycastType.MaxCount:
                    return new MaxCount(area, config.Params);
                default:
                    throw new CombatException("Raycast type doesn't exist: " + config.Type);
            }
        }
    }
}
