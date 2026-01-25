using Framework;
using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("时间/等待")]
    public class WaitForTime : EffectNode {
        public override string name => "等待";
        [OdinTree] public Combat.Skill.SkillNode.WaitForTime.Param param;
        protected override object Params => param;
    }
    
    [Category("时间/等待普通攻击前摇")]
    public class WaitForAttackWindup : EffectNode {
        public override string name => "等待普通攻击前摇";
        protected override object Params => null;
    }
    
    [Category("时间/等待普通攻击后摇")]
    public class WaitForAttackBackswing : EffectNode {
        public override string name => "等待普通攻击后摇";
        protected override object Params => null;
    }
}