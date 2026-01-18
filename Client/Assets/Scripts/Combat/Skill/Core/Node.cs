namespace Combat.Skill {
    public enum NodeState {
        NoKnow = -1,
        Continue,
        Finish,
        Fail,
    }
    
    public abstract class Node {
        public virtual NodeState OnEnter(Context context) => NodeState.NoKnow;
        public virtual NodeState OnUpdate(Context context) => NodeState.NoKnow;
        public virtual void OnExit(Context context) { }
        public virtual void OnFinish(Context context) { }
        public virtual void OnFail(Context context) { }
    }
}