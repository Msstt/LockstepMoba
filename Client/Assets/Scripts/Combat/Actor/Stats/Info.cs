using System;
using Sirenix.OdinInspector;

namespace Combat.Actor {
    [Serializable]
    public class DamageInfo {
        [LabelText("物理伤害")]
        public StatScaler Physical;
        [LabelText("魔法伤害")]
        public StatScaler Magic;
        [LabelText("真实伤害")]
        public StatScaler True;
    }
}