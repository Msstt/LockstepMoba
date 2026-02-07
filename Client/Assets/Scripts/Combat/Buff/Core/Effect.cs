using Newtonsoft.Json.Linq;

namespace Combat.Buff {
    public interface IEffect {
        public void OnCreate() { }
        public void OnRefresh() { }
        public void OnUpdate() { }
        public void OnDestroy() { }
    }
    
    public abstract class Effect<Param> : IEffect {
        private Buff buff;
        private Param param;
        
        protected Effect(Buff buff, JToken json) {
            this.buff = buff;
            param = json.ToObject<Param>();
            if (param == null) {
                throw new CombatException($"Node ParseParam {typeof(Param).Name} is null");
            }
        }
    }
}