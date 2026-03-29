using ParadoxNotion.Design;

namespace Editor.Buff.Effect {
    [Category("属性/改变攻击力")]
    public class ChangeAttack : EffectNode<Combat.Buff.Effect.ChangeAttack.Param> {
        public override string name => "改变攻击力";
    }
}