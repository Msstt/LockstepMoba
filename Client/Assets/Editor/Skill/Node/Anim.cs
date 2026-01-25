using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("动画/播放动画")]
    public class PlayAnim : EffectNode {
        public override string name => "播放动画";
        [OdinTree] public Combat.Skill.SkillNode.PlayAnim.Param param;
        protected override object Params => param;
    }
}