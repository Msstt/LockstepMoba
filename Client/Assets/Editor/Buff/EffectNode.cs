using ParadoxNotion.Design;

namespace Editor.Buff {
    [Category("伤害/持续伤害")]
    public class Damage : EffectNode<Combat.Buff.Effect.Damage.Param> {
        public override string name => "持续伤害";
    }
}