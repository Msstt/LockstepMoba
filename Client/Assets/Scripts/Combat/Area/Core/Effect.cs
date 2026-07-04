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
        private int raycastId;
        
        protected Effect(Area area, int raycastId, JToken json) {
            this.area = area;
            this.raycastId = raycastId;
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
            List<Actor.Actor> actors = area.Raycast(raycastId);
            foreach (Actor.Actor actor in actors) {
                func(actor);
            }
        }
    }
}