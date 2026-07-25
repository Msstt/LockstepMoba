using ParadoxNotion.Design;

namespace Editor.Area {
    [Category("通用")]
    public class DestroyOnHit : EffectNode<Combat.Area.Effect.DestroyOnHit.Param> {
        public override string name => "碰撞后销毁";
    }
    
    [Category("通用")]
    public class AddBuff : EffectNode<Combat.Area.Effect.AddBuff.Param> {
        public override string name => "施加 Buff";
    }
}