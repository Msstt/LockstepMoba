namespace Combat.Actor {
    public class Stats {
        public LimitedPriority Health;
        
        public Priority Attack;
        public Priority AttackSpeed;
        public Priority AttackDistance;
        
        public Priority MoveSpeed;
    }
    
    public partial class Const {
        public readonly FloatF AttackWindupRatio;
    }
}