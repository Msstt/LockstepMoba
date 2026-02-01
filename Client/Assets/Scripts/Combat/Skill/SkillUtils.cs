using System;

public static class SkillUtils {
    public static readonly int SkillSlotCount = Enum.GetNames(typeof(Combat.Skill.SkillSlot)).Length;
}
