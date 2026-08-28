using System;
using System.Collections.Generic;
using Combat.Actor;
using Framework;
using Newtonsoft.Json.Linq;

namespace Combat.Area {
    public interface IEffect : ICheckableData {
        public void OnCreate();
        public void OnUpdate();
        public void OnRenderUpdate();
        public void OnDestroy();
        
        // 内部的优先级，无视配置，比如位置更新类的 Effect 必须先 Update
        public int Priority { get; }
    }
    
    public abstract class Effect<Param> : IEffect, ICheckableData {
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

        public virtual int Priority => 0;

        protected T GetLevelNumber<T>(LevelNumber<T> levelNumber) => levelNumber[area.Level];

        protected void Raycast(Action<Actor.Actor> func) {
            List<Actor.Actor> actors = area.Raycast(raycastId);
            foreach (Actor.Actor actor in actors) {
                func(actor);
            }
        }

        public virtual int GetStatusCode() => 0;
    }

    public abstract class MoveEffect<Param> : Effect<Param> {
        protected MoveEffect(Area area, int raycastId, JToken json) : base(area, raycastId, json) { }
        
        public override int Priority => -10;
    }
}
