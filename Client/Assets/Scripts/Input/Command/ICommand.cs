using Network;

namespace InputSystem.Command {
    public interface ICommand {
        public void Update();
        
        public skill_param GetProto();
    }
}