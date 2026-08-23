using ParadoxNotion.Design;

namespace Editor.Area {
    [Category("移动")]
    public class LinearMove : EffectNode<Combat.Area.Effect.LinearMove.Param> {
        public override string name => "直线移动";
    }
    
    [Category("移动")]
    public class TargetActorMove : EffectNode<Combat.Area.Effect.TargetActorMove.Param> {
        public override string name => "向目标单位移动";
    }
}