using Combat.Actor.Skill;
using Framework;
using InputSystem.Command;
using Network;

namespace InputSystem {
    public class InputSystem : IInputSystem {
        private ICommand[] commands = new ICommand[6];

        public void Init() {
            EventMgr.Instance.Register<EventType.OnLockStepStart>(() => {
                NetworkUtils.RegisterCollector(MessageDef.skill_input, Collector);
            });
        }
        
        public void Start() {
            ChangeCommand(SkillSlot.Move, new MoveCommand());
        }

        public void Update() {
            foreach (var command in commands) {
                command?.Update();
            }
        }

        public void ChangeCommand(SkillSlot slot, ICommand command) {
            commands[(int)slot] = command;
        }

        private skill_input Collector() {
            var msg = new skill_input();
            for (int i = 0; i < commands.Length; i++) {
                var command = commands[i];
                var param = command?.GetProto();
                if (param != null) {
                    msg.Info.Add(new skill_info {
                        Slot = i,
                        Param = param,
                    });
                }
            }
            return msg;
        }
    }
}