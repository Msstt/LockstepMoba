namespace Combat.Skill {
    public abstract class SelectNode : Node {
        public static readonly int InValidIndex = -1;
        
        public virtual NodeState OnEnter(Context context) => NodeState.Finish;
        public virtual NodeState OnUpdate(Context context) => NodeState.Finish;

        public abstract int Select(Context context);
    }
}