using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("通用/中断技能")]
    public class AbortSkill : EffectNode {
        public override string name => "中断技能";
        [OdinTree] public Combat.Skill.SkillNode.AbortSkill.Param param;
        protected override object Params => param;
    }
    
    [Category("通用/请求槽位")]
    public class RequestSlot : EffectNode {
        public override string name => "请求槽位";
        [OdinTree] public Combat.Skill.SkillNode.RequestSlot.Param param;
        protected override object Params => param;
    }
}