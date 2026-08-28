using System;
using System.Collections.Generic;
using Framework;

namespace Combat.Buff {
    public class Buff : IDisposable, ICheckableData {
        private List<IEffect> effects = new List<IEffect>();

        public int AdderId { get; private set; }
        public int ActorId { get; private set; }
        public int Id { get; private set; }
        public int Level { get; private set; }
        
        public int Count { get; private set; }
        private readonly int maxCount;

        public Buff(int buffId, int actorId, int adderId, int level) {
            Id = buffId;
            ActorId = actorId;
            AdderId = adderId;
            Level = level;
            Count = 1;
            
            BuffConfig config = Config.Buff[buffId];
            maxCount = config.MaxCount;
            foreach (EffectConfig effect in config.Effect) {
                effects.Add(EffectFactory.CreateEffect(this, effect));
            }
        }

        public void Init() {
            ExecuteEffect((effect) => effect.OnCreate());
            ExecuteEffect((effect) => effect.OnRefresh());
        }

        public void Merge(int adderId, int level) {
            AdderId = adderId;
            Level = level;
            Count = Math.Min(Count + 1, maxCount);
            ExecuteEffect((effect) => effect.OnRefresh());
        }
        
        public bool Reduce() {
            Count = Math.Max(Count - 1, 0);
            if (Count == 0) {
                Dispose();
                return true;
            }
            return false;
        }
        
        public void Update() {
            ExecuteEffect((effect) => effect.OnUpdate());
        }

        public void Dispose() {
            ExecuteEffect((effect) => effect.OnDestroy());
            effects.Clear();
        }
        
        private void ExecuteEffect(Action<IEffect> func) {
            foreach (IEffect effect in effects) {
                func(effect);
            }
        }

        public int GetStatusCode() {
            int code = StatusCode.Seed;
            code = StatusCode.Combine(code, Id);
            code = StatusCode.Combine(code, ActorId);
            code = StatusCode.Combine(code, AdderId);
            code = StatusCode.Combine(code, Level);
            code = StatusCode.Combine(code, Count);
            code = StatusCode.Combine(code, maxCount);
            code = StatusCode.Combine(code, effects.Count);
            foreach (IEffect effect in effects) {
                code = StatusCode.CombineData(code, effect);
            }
            return code;
        }
    }
}
