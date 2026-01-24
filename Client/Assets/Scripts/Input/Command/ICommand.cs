// TODO 技能释放过程被打断

using Network;

namespace InputSystem.Command {
    public interface ICommand {
        public void Update();
        
        public skill_param GetProto();
    }
}