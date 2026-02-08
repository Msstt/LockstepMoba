using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("通用/中断技能")]
    public class AbortSkill : ParamNode<Combat.Skill.SkillNode.AbortSkill.Param> {
        public override string name => "中断技能";
    }
    
    [Category("通用/请求槽位")]
    public class RequestSlot : ParamNode<Combat.Skill.SkillNode.RequestSlot.Param> {
        public override string name => "请求槽位";
    }
    
    [Category("通用/重复此技能")]
    public class RepeatSkill : NoParamNode {
        public override string name => "重复此技能";
    }
    
    [Category("通用/空")]
    public class None : NoParamNode {
        public override string name => "";
    }
}