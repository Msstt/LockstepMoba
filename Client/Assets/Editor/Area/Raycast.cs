using ParadoxNotion.Design;

namespace Editor.Area {
    [Category("检测")]
    public class All : RaycastNode<Combat.Area.Raycast.All.Param> {
        public override string name => "所有";
    }
    
    [Category("检测")]
    public class AllByType : RaycastNode<Combat.Area.Raycast.AllByType.Param> {
        public override string name => "指定类型";
    }
    
    [Category("检测")]
    public class MaxCount : RaycastNode<Combat.Area.Raycast.MaxCount.Param> {
        public override string name => "指定数量";
    }
    
    [Category("检测")]
    public class ArriveTargetUid : RaycastNode<Combat.Area.Raycast.ArriveTargetUid.Param> {
        public override string name => "到达指定单位";
    }
}