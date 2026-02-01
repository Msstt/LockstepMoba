using System.Collections.Generic;
using Combat.Skill;
using UnityEngine;

namespace InputSystem {
    public enum CommandType {
        None,
        SinglePos,
    }
    
    public static class Config {
        public static Dictionary<SkillSlot, KeyCode> Key = new Dictionary<SkillSlot, KeyCode> {
            { SkillSlot.SkillQ, KeyCode.Q },
            { SkillSlot.SkillW, KeyCode.W },
            { SkillSlot.SkillE, KeyCode.E },
            { SkillSlot.SkillR, KeyCode.R },
            { SkillSlot.SkillD, KeyCode.D },
            { SkillSlot.SkillF, KeyCode.F },
        };
    }
}