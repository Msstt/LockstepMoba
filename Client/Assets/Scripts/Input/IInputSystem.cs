using Combat.Skill;
using InputSystem.Command;

namespace InputSystem {
    public interface IInputSystem : IInitSystem, IStartSystem, IUpdateSystem {
        public void ChangeCommand(SkillSlot slot, ICommand command);
    }
}