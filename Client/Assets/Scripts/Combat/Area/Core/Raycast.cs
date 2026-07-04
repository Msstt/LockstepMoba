using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Combat.Area {
    public interface IRaycast {
        public List<Actor.Actor> Get();
    }
    
    public abstract class Raycast<Param> : IRaycast {
        protected Area area;
        protected Param param;
        
        protected Raycast(Area area, JToken json) {
            this.area = area;
            param = json.ToObject<Param>();
            if (param == null) {
                throw new CombatException($"Area Effect ParseParam {typeof(Param).Name} is null");
            }
        }

        public virtual List<Actor.Actor> Get() => new List<Actor.Actor>();
    }
}