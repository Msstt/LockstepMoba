using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Combat.Area {
    public class EffectConfig {
        public EffectType Type;
        public JToken Params;
    }
    
    public class AreaConfig {
        public int Id;
        public string Name;
        public string Prefab;
        public ShapeType ShapeType;
        public JToken ShapeParam;
        public FloatF Time;
        public List<EffectConfig> Effect;
    }
}