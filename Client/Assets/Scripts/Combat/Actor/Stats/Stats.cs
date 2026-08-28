namespace Combat.Actor {
    public class Stats : ICheckableData {
        public LimitedPriority Health;
        
        public Priority Attack;
        public Priority AttackSpeed;
        public Priority AttackDistance;
        
        public Priority MoveSpeed;
        
        public Priority Radius;

        public int Invisibility; // == 0 表示可见

        public int GetStatusCode() {
            int code = Framework.StatusCode.Seed;
            code = Framework.StatusCode.CombineData(code, Health);
            code = Framework.StatusCode.CombineData(code, Attack);
            code = Framework.StatusCode.CombineData(code, AttackSpeed);
            code = Framework.StatusCode.CombineData(code, AttackDistance);
            code = Framework.StatusCode.CombineData(code, MoveSpeed);
            code = Framework.StatusCode.CombineData(code, Radius);
            return Framework.StatusCode.Combine(code, Invisibility);
        }
    }
}
