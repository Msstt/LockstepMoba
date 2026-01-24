// 技能树节点：按路径移动到指定位置

namespace Combat.Skill {
    public class MoveToPosByPath : Node {
        public virtual NodeState OnEnter(Context context) => NodeState.NoKnow;
        public virtual NodeState OnUpdate(Context context) => NodeState.NoKnow;
        public virtual void OnExit(Context context) { }
        public virtual void OnFail(Context context) { }
    }
}