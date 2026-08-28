using Combat.Actor;
using Framework;
using Newtonsoft.Json.Linq;

namespace Combat.Buff {
    public interface IEffect : ICheckableData {
        public void OnCreate();
        public void OnRefresh();
        public void OnUpdate();
        public void OnDestroy();
    }
    
    public abstract class Effect<Param> : IEffect {
        protected Buff buff;
        protected Param param;
        
        protected Effect(Buff buff, JToken json) {
            this.buff = buff;
            param = json.ToObject<Param>();
            if (param == null) {
                throw new CombatException($"Buff Effect ParseParam {typeof(Param).Name} is null");
            }
        }
        
        public virtual void OnCreate() { }
        public virtual void OnRefresh() { }
        public virtual void OnUpdate() { }
        public virtual void OnDestroy() { }

        protected T GetLevelNumber<T>(LevelNumber<T> levelNumber) => levelNumber[buff.Level];

        public virtual int GetStatusCode() => StatusCode.Seed;
    }
}
