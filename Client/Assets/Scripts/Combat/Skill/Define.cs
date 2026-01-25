using System;

namespace Combat.Skill {
    public enum SkillSlot {
        Move = 0,
        Attack = 1,
        Skill1 = 2,
        Skill2 = 3,
        Skill3 = 4,
        Skill4 = 5,
    }
    
    [Flags] [Serializable]
    public enum SkillType {
        Move = 1 << 0,
        NormalAttack = 1 << 1,
    }

    public static class Field {
        public static readonly string OverrideSkillType = "OverrideSkillType";
    }
}