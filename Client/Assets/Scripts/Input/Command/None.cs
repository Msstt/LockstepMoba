using Network;

namespace InputSystem.Command {
    public class None : Command {
        public override void Update() { }
        public override skill_param GetProto() => null;
    }
}