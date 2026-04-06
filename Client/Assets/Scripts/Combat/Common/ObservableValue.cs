using System;
using Framework;
using UnityEngine.UI;

namespace Combat {
    public interface IReadOnlyObservableValue<T> {
        T Value { get; }
        public event Action<T> OnValueChanged;
    }
    
    public class ObservableValue<T> : IReadOnlyObservableValue<T> where T : struct {
        public ObservableValue(T defaultValue) {
            rawValue = defaultValue;
        }
        
        private T rawValue = default(T);
        public T Value {
            get => rawValue;
            set {
                if (!Equals(rawValue, value)) {
                    rawValue = value;
                    onValueChanged.Send(rawValue);
                }
            }
        }
        
        private SafeEvent<T> onValueChanged = new SafeEvent<T>();
        public event Action<T> OnValueChanged {
            add {
                value?.Invoke(rawValue);
                onValueChanged?.Register(value);
            }
            remove { onValueChanged?.UnRegister(value); }
        }
    }
}