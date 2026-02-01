using ParadoxNotion.Design;

namespace Editor.Skill {
    public class Cost {
        [Category("消耗/设置冷却")]
        public class SetCD : EffectNode {
            public override string name => "设置冷却";
            [OdinTree] public Combat.Skill.SkillNode.SetCD.Param param;
            protected override object Params => param;
        }
    }
}