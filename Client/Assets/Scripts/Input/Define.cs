using System.Collections.Generic;
using Combat.Skill;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InputSystem {
    [DrawWithUnity]
    public enum CommandType {
        [InspectorName("空")]
        None,
        [InspectorName("单一位置")]
        SinglePos,
        [InspectorName("单一目标")]
        SingleUnit,
    }
    
    public static class Config {
        public static Dictionary<SkillSlot, KeyCode> Key = new Dictionary<SkillSlot, KeyCode> {
            { SkillSlot.SkillQ, KeyCode.Q },
            { SkillSlot.SkillW, KeyCode.W },
            { SkillSlot.SkillE, KeyCode.E },
            { SkillSlot.SkillR, KeyCode.R },
            { SkillSlot.SkillD, KeyCode.D },
            { SkillSlot.SkillF, KeyCode.F },
            { SkillSlot.Equipment1, KeyCode.Alpha1 },
            { SkillSlot.Equipment2, KeyCode.Alpha2 },
            { SkillSlot.Equipment3, KeyCode.Alpha3 },
            { SkillSlot.Equipment4, KeyCode.Alpha4 },
            { SkillSlot.Equipment5, KeyCode.Alpha5 },
            { SkillSlot.Equipment6, KeyCode.Alpha6 },
        };
    }
}