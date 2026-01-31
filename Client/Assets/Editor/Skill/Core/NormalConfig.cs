using Combat.Skill;
using Sirenix.OdinInspector;

namespace Editor.Skill {
    public class NormalConfig {
        [ReadOnly]
        public int Id;
        public string Name;
        [DrawWithUnity]
        public SkillType SkillType;
        public bool CanAbortSelf;
    }
}