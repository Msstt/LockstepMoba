using Google.Protobuf;
using Network;

namespace Battle {
    public class InputMerge {
        private delegate IMessage MergeFunc(IMessage lastInput, IMessage input);
        private static readonly Dictionary<string, MergeFunc> mergeFunc = new Dictionary<string, MergeFunc>() {
            { "test", Bind<test_input>(Test) }
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
    }
}
