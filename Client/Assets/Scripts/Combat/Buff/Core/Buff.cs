using System;
using System.Collections.Generic;

namespace Combat.Buff {
    public class Buff : IDisposable {
        private List<IEffect> effects = new List<IEffect>();

        public int AdderId { get; private set; }
        public int ActorId { get; private set; }
        public int Level { get; private set; }
        
        public int Count { get; private set; }
        private readonly int maxCount;

        public Buff(int buffId, int actorId, int adderId, int level) {
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
        
        public void Update() {
            ExecuteEffect((effect) => effect.OnUpdate());
        }

        public void Dispose() {
            effects.Clear();
            ExecuteEffect((effect) => effect.OnDestroy());
        }
        
        private void ExecuteEffect(Action<IEffect> func) {
            foreach (IEffect effect in effects) {
                func(effect);
            }
        }
    }
}