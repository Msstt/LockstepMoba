using System;
using System.Collections.Generic;

namespace Combat {
    [Serializable]
    public class MinionWave {
        public List<SimpleTransform> Pos = new List<SimpleTransform>();
    }
}