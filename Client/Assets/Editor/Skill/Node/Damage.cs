using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("伤害/对参数目标造成伤害")]
    public class DamageToSingle : EffectNode {
        public override string name => "对参数目标造成伤害";
        [OdinTree] public Combat.Skill.SkillNode.DamageToSingle.Param param;
        protected override object Params => param;
    }
}