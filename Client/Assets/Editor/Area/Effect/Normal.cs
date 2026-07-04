using ParadoxNotion.Design;

namespace Editor.Area {
    [Category("通用/碰撞后销毁")]
    public class DestroyOnHit : EffectNode<Combat.Area.Effect.DestroyOnHit.Param> {
        public override string name => "碰撞后销毁";
    }
}