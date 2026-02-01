using System.Collections.Generic;
using Combat;
using Combat.Skill;
using Framework;
using InputSystem.Command;
using Network;
using UnityEngine;

namespace InputSystem {
    public class InputSystem : IInputSystem {
        private Command.Command[] commands = new Command.Command[SkillUtils.SkillSlotCount];
        private bool[] isEnable = new bool[SkillUtils.SkillSlotCount];

        public void Init() {
            EventMgr.Instance.Register<EventType.OnLockStepStart>(() => {
                NetworkUtils.RegisterCollector(MessageDef.skill_input, Collector);
            });
        }
        
        public void Start() {
            commands[(int)SkillSlot.Move] = new Move();
            commands[(int)SkillSlot.Attack] = new Attack();
        }

        public void Update() {
            foreach (var command in commands) {
                command?.Update();
            }
        }

        public void ChangeCommand(SkillSlot slot, CommandType type) {
            if (slot is SkillSlot.Move or SkillSlot.Attack) {
                return;
            }
            
            commands[(int)slot]?.OnSuspend();
            
            isEnable[(int)slot] = true;
            KeyCode key = Config.Key[slot];
            switch (type) {
                case CommandType.None:
                    commands[(int)slot] = new None(); break;
                case CommandType.SinglePos:
                    commands[(int)slot] = new SinglePos(key); break;
                default:
                    Log.Error("InputSystem ChangeCommand Unknown CommandType"); break;
            }
        }

        public void EnableCommand(SkillSlot slot, bool enable) {
            if (isEnable[(int)slot] == enable) {
                return;
            }
            isEnable[(int)slot] = enable;
            if (!enable) {
                commands[(int)slot].OnSuspend();
            }
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