using Combat.Actor;
using Newtonsoft.Json.Linq;

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
        
        protected void SetValue<T>(Context context, string key, T value) => context.SetValue(Id, key, value);
        protected T GetValue<T>(Context context, string key) => context.GetValue<T>(Id, key);
        protected T GetValueOrDefault<T>(Context context, string key, T defaultValue) => context.GetValueOrDefault(Id, key, defaultValue);
        protected static void SetGlobalValue<T>(Context context, string key, T value) => context.SetValue(key, value);
        protected static T GetGlobalValue<T>(Context context, string key) => context.GetValue<T>(key);
        protected static T GetGlobalValueOrDefault<T>(Context context, string key, T defaultValue) => context.GetValueOrDefault(key, defaultValue);
        
        protected static T GetCom<T>(Context context) where T : Com => ActorUtils.GetCom<T>(context.ActorUid);
        protected static Stats GetStats(Context context) => ActorUtils.GetActor(context.ActorUid)?.Stats;

        protected static T ParseParam<T>(JToken json) {
            T param = json.ToObject<T>();
            if (param == null) {
                throw new CombatException($"Node ParseParam {typeof(T).Name} is null");
            }
            return param;
        }
    }
}