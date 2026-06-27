using ParadoxNotion.Design;

namespace Editor.Area {
    [Category("移动/直线移动")]
    public class LinearMove : EffectNode<Combat.Area.Effect.LinearMove.Param> {
        public override string name => "直线移动";
    }
}