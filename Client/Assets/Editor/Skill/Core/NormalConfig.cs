using Combat.Skill;
using Sirenix.OdinInspector;

namespace Editor.Skill {
    public class NormalConfig {
        [ReadOnly]
        public int Id;
        [LabelText("名称")]
        public string Name;
        [DrawWithUnity]
        [LabelText("技能类型")]
        public SkillType SkillType;
        [LabelText("是否可自我打断")]
        public bool CanAbortSelf;
    }
}