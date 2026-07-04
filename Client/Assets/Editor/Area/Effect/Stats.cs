using ParadoxNotion.Design;

namespace Editor.Area {
    [Category("伤害/持续治疗")]
    public class Heal : EffectNode<Combat.Area.Effect.Heal.Param> {
        public override string name => "持续治疗";
    }
    
    [Category("伤害/持续伤害")]
    public class Damage : EffectNode<Combat.Area.Effect.Damage.Param> {
        public override string name => "持续伤害";
    }
}