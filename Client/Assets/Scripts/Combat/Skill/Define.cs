using System;

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
        None = 0,
        Move = 1 << 0,
        NormalAttack = 1 << 1,
        Flash = 1 << 2,
    }

    public static class Field {
        public static readonly string OverrideSkillType = "OverrideSkillType";
    }
}