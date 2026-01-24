using System.Collections.Generic;
using Combat.Skill;
using Framework;
using InputSystem.Command;
using Network;

namespace InputSystem {
    public class InputSystem : IInputSystem {
        private ICommand[] commands = new ICommand[SkillUtils.SkillSlotCount];

        public void Init() {
            EventMgr.Instance.Register<EventType.OnLockStepStart>(() => {
                NetworkUtils.RegisterCollector(MessageDef.skill_input, Collector);
            });
        }
        
        public void Start() {
            ChangeCommand(SkillSlot.Move, new MoveCommand());
            ChangeCommand(SkillSlot.Attack, new AttackCommand());
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
            SortedDictionary<SkillSlot, skill_info> infos = new SortedDictionary<SkillSlot, skill_info>();
            for (int i = commands.Length - 1; i >= 0; i--) {
                var command = commands[i];
                var param = command?.GetProto();
                if (param != null) {
                    infos[(SkillSlot)i] = new skill_info {
                        Slot = i,
                        Param = param,
                    };
                }
            }

            // Move Attack 特殊处理，因为都绑定右键，所以在这里去重
            if (infos.ContainsKey(SkillSlot.Attack)) {
                infos.Remove(SkillSlot.Move);
            }

            foreach (var info in infos.Values) {
                msg.Info.Add(info);
            }
            return msg;
        }
    }
}