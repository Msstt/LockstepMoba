namespace Combat.Actor {
    public class StatScaler {
        public FloatF Value;
        public FloatF AttackScale;
    }
    
    public static class StatsUtils {
        public static FloatF GetValue(Stats stats, StatScaler scaler) {
            FloatF value = scaler.Value;
            value += stats.Attack * scaler.AttackScale;
            return value;
        }
    }
}