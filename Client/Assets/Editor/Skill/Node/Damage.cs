using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("伤害/对参数目标造成伤害")]
    public class DamageToSingle : ParamNode<Combat.Actor.DamageInfo> {
        public override string name => "对参数目标造成伤害";
    }
}