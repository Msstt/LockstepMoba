using System;
using Sirenix.OdinInspector;
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
        Equipment1 = 8,
        Equipment2 = 9,
        Equipment3 = 10,
        Equipment4 = 11,
        Equipment5 = 12,
        Equipment6 = 13,
    }
    
    [Flags] [Serializable]
    [DrawWithUnity]
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