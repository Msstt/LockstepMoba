// TODO 技能释放过程被打断

using Network;

namespace InputSystem.Command {
    public abstract class Command {
        public abstract void Update();
        
        public abstract skill_param GetProto();
        
        public void OnSuspend() { }
    }
}