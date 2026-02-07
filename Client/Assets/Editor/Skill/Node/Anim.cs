using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("动画/播放动画")]
    public class PlayAnim : ParamNode<Combat.Skill.SkillNode.PlayAnim.Param> {
        public override string name => "播放动画";
    }
}