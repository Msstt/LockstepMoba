using ParadoxNotion.Design;

namespace Editor.Buff.Effect {
    [Category("可见性/隐身")]
    public class Invisibility : EffectNode<Combat.Buff.Effect.Invisibility.Param> {
        public override string name => "隐身";
    }
    
    [Category("可见性/潜行")]
    public class InvisibilityByRadius : EffectNode<Combat.Buff.Effect.InvisibilityByRadius.Param> {
        public override string name => "潜行";
    }
}