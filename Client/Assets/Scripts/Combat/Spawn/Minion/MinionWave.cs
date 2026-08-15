using System;
using System.Collections.Generic;

namespace Combat.Actor {
    [Serializable]
    public class MinionWave {
        public List<SimpleTransform> Pos = new List<SimpleTransform>();
    }
}