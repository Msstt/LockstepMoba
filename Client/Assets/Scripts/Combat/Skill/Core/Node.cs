using Combat.Actor;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Combat.Skill {
    public enum NodeState {
        NoKnow = -1,
        Continue,
        Finish,
        Fail,
    }
    
    public abstract class Node {
        private static bool PrintSkillTree = false;
        private static int globalId = 0;
        
        public int Id { get; private set; }

        protected Node() {
            Id = globalId++;

            PrintSkillTree = GameMgr.Instance.GMTool.PrintSkillTree;
        }
        
        protected virtual NodeState OnEnter(Context context) => NodeState.NoKnow;
        protected virtual NodeState OnUpdate(Context context) => NodeState.NoKnow;
        protected virtual void OnExit(Context context) { }
        protected virtual void OnFinish(Context context) { }
        protected virtual void OnFail(Context context) { }

        public NodeState Enter(Context context) {
            Log("Enter", context);
            return OnEnter(context);
        }
        
        public NodeState Update(Context context) {
            return OnUpdate(context);
        }

        public void Exit(Context context) {
            Log("Exit", context);
            OnExit(context);
        }

        public void Finish(Context context) {
            Log("Finish", context);
            OnFinish(context);
        }

        public void Fail(Context context) {
            Log("Fail", context);
            OnFail(context);
        }

        private void Log(string message, Context context) {
            if (PrintSkillTree) {
                Debug.Log("[SkillTree] " + GameMgr.Instance.Frame + ": " + message + 
                          " Node: " + GetType().Name + 
                          " TreeId: " + context.TreeId + 
                          " ActorUid: " + context.ActorUid);
            }
        }
        
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

        protected static T GetLevelNumber<T>(Context context, LevelNumber<T> levelNumber) => levelNumber[context.Level];
    }
}