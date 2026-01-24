
namespace Combat.Skill {
    public enum NodeState {
        NoKnow = -1,
        Continue,
        Finish,
        Fail,
    }
    
    public abstract class Node {
        private static int globalId = 0;
        
        public int Id { get; private set; }

        protected Node() {
            Id = globalId++;
        }
        
        public virtual NodeState OnEnter(Context context) => NodeState.NoKnow;
        public virtual NodeState OnUpdate(Context context) => NodeState.NoKnow;
        public virtual void OnExit(Context context) { }
        public virtual void OnFinish(Context context) { }
        public virtual void OnFail(Context context) { }
        
        public void SetValue<T>(Context context, string key, T value) => context.SetValue(Id, key, value);
        public T GetValue<T>(Context context, string key) => context.GetValue<T>(Id, key);
        public T GetValueOrDefault<T>(Context context, string key, T defaultValue) => context.GetValueOrDefault(Id, key, defaultValue);
        public void SetGlobalValue<T>(Context context, string key, T value) => context.SetValue(key, value);
        public T GetGlobalValue<T>(Context context, string key) => context.GetValue<T>(key);
        public T GetGlobalValueOrDefault<T>(Context context, string key, T defaultValue) => context.GetValueOrDefault(key, defaultValue);
    }
}