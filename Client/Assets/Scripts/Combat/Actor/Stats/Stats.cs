namespace Combat.Actor {
    public class Stats {
        public LimitedPriority Health;
        
        public Priority Attack;
        public Priority AttackSpeed;
        public Priority AttackDistance;
        
        public Priority MoveSpeed;
        
        public Priority Radius;

        public int Invisibility; // == 0 表示可见
    }
}