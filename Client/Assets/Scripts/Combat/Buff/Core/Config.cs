using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Combat.Buff {
    public class EffectConfig {
        public EffectType Type;
        public JToken Params;
    }
    
    public class BuffConfig {
        public int Id;
        public string Name;
        public bool IsForever;
        public FloatF Time;
        public bool IsOnly;
        public int MaxCount;
        public List<EffectConfig> Effect;
    }
}