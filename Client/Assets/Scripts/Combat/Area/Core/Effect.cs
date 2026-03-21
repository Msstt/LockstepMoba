using System;
using System.Collections.Generic;
using Combat.Actor;
using Newtonsoft.Json.Linq;

namespace Combat.Area {
    public interface IEffect {
        public void OnCreate();
        public void OnUpdate();
        public void OnRenderUpdate();
        public void OnDestroy();
    }
    
    public abstract class Effect<Param> : IEffect {
        protected Area area;
        protected Param param;
        
        protected Effect(Area area, JToken json) {
            this.area = area;
            param = json.ToObject<Param>();
            if (param == null) {
                throw new CombatException($"Area Effect ParseParam {typeof(Param).Name} is null");
            }
        }
        
        public virtual void OnCreate() { }
        public virtual void OnUpdate() { }
        public virtual void OnRenderUpdate() { }
        public virtual void OnDestroy() { }

        protected T GetLevelNumber<T>(LevelNumber<T> levelNumber) => levelNumber[area.Level];

        protected void Raycast(Action<Actor.Actor> func) {
            List<int> actors = area.Shape.Raycast(area.Position, area.Direction);
            for (int i = 0; i < actors.Count; i++) {
                Actor.Actor actor = ActorUtils.GetActor(actors[i]);
                if (actor != null) {
                    func(actor);
                }
            }
        }
    }
}