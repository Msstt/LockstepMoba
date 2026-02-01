using Combat.Skill;
using InputSystem.Command;

namespace InputSystem {
    public interface IInputSystem : IInitSystem, IStartSystem, IUpdateSystem {
        public void ChangeCommand(SkillSlot slot, CommandType type);

        public void EnableCommand(SkillSlot slot, bool enable);
    }
}