namespace Combat.Skill {
    public abstract class SelectNode : Node {
        public NodeState OnEnter(Context context) => NodeState.Finish;
        public NodeState OnUpdate(Context context) => NodeState.Finish;

        public virtual int Select(Context context) => 0;
    }
}