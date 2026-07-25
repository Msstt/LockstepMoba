using ParadoxNotion.Design;

namespace Editor.Buff.Effect {
    [Category("属性")]
    public class ChangeAttack : EffectNode<Combat.Buff.Effect.ChangeAttack.Param> {
        public override string name => "改变攻击力";
    }
    
    [Category("属性")]
    public class Damage : EffectNode<Combat.Buff.Effect.Damage.Param> {
        public override string name => "持续伤害";
    }
}