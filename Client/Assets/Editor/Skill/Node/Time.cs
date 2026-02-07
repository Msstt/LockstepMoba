using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("时间/等待")]
    public class WaitForTime : ParamNode<Combat.Skill.SkillNode.WaitForTime.Param> {
        public override string name => "等待";
    }
    
    [Category("时间/等待普通攻击前摇")]
    public class WaitForAttackWindup : NoParamNode {
        public override string name => "等待普通攻击前摇";
    }
    
    [Category("时间/等待普通攻击后摇")]
    public class WaitForAttackBackswing : NoParamNode {
        public override string name => "等待普通攻击后摇";
    }
}