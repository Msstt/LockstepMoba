using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("动画")]
    public class PlayAnim : ParamNode<Combat.Skill.SkillNode.PlayAnim.Param> {
        public override string name => "播放动画";
    }
    
    [Category("动画")]
    public class SetDirByInputParam : NoParamNode {
        public override string name => "根据输入参数设置角色朝向";
    }
}