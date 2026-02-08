using System;
using UnityEngine;

namespace Combat.Skill {
    public enum SkillSlot {
        Move = 0,
        Attack = 1,
        SkillQ = 2,
        SkillW = 3,
        SkillE = 4,
        SkillR = 5,
        SkillD = 6,
        SkillF = 7,
    }
    
    [Flags] [Serializable]
    public enum SkillType {
        [InspectorName("无")]
        None = 0,
        [InspectorName("移动")]
        Move = 1 << 0,
        [InspectorName("普通攻击")]
        NormalAttack = 1 << 1,
        [InspectorName("闪现")]
        Flash = 1 << 2,
        [InspectorName("脱手")]
        Sell = 1 << 3,
    }

    public static class Field {
        public static readonly string OverrideSkillType = "OverrideSkillType";
    }
}