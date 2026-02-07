using System;
using Sirenix.OdinInspector;

namespace Combat.Actor {
    [Serializable]
    public class DamageInfo {
        [LabelText("物理伤害")]
        public LevelNumber<StatScaler> Physical;
    }
}