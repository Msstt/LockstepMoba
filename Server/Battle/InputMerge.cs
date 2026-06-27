using Google.Protobuf;
using Google.Protobuf.Collections;
using Network;

namespace Battle {
    public class InputMerge {
        private delegate IMessage MergeFunc(IMessage lastInput, IMessage input);
        private static readonly Dictionary<string, MergeFunc> mergeFunc = new Dictionary<string, MergeFunc>() {
            { "test", Bind<test_input>(Test) },
            { "skill", Bind<skill_input>(Skill) },
            { "level", Bind<level_input>(Level) },
        };

        private static MergeFunc Bind<T>(Func<T, T, T> func) where  T : class, IMessage {
            return (lastInput, input) => {
                if (lastInput == null) {
                    return input;
                }
                if (input == null) {
                    return lastInput;
                }
                return func?.Invoke(lastInput as T, input as T);
            };
        }
        
        public static battle_input Merge(battle_input lastInput, battle_input input) {
            battle_input msg = new battle_input();
            
            foreach (var field in battle_input.Descriptor.Fields.InFieldNumberOrder()) {
                var msg1 = field.Accessor.GetValue(lastInput) as IMessage;
                var msg2 = field.Accessor.GetValue(input) as IMessage;
                
                field.Accessor.SetValue(msg, mergeFunc[field.Name](msg1, msg2));
            }

            return msg;
        }
        
        private static test_input Test(test_input lastInput, test_input input) {
            return new test_input {
                Count = lastInput.Count + input.Count,
            };
        }
        
        // 移动、普攻只保留一次输入
        private static skill_input Skill(skill_input lastInput, skill_input input) {
            bool[] skillSlot = new bool[Enum.GetValues(typeof(SkillSlot)).Length];
            List<skill_info> infos = new List<skill_info>();

            void Add(RepeatedField<skill_info> info) {
                for (int i = info.Count - 1; i >= 0; i--) {
                    int slot = info[i].Slot;
                    if (slot < 0 || slot >= skillSlot.Length) {
                        continue;
                    }

                    if ((slot == (int)SkillSlot.Move || slot == (int)SkillSlot.Attack) && skillSlot[slot]) {
                        continue;
                    }
                    skillSlot[slot] = true;
                    infos.Add(info[i]);
                }
            }
            
            Add(input.Info);
            Add(lastInput.Info);

            infos.Reverse();
            skill_input msg = new skill_input();
            foreach (var info in infos) {
                msg.Info.Add(info);
            }
            return msg;
        }

        private static level_input Level(level_input lastInput, level_input input) {
            level_input msg = new level_input();
            foreach (var info in lastInput.LevelUp) {
                msg.LevelUp.Add(info);
            }
            foreach (var info in input.LevelUp) {
                msg.LevelUp.Add(info);
            }
            return msg;
        }
    }
}
