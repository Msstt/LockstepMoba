using ParadoxNotion.Design;

namespace Editor.Area {
    [Category("伤害/持续治疗")]
    public class Heal : EffectNode<Combat.Area.Effect.Heal.Param> {
        public override string name => "持续治疗";
    }
}