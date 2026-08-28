using System;
using Framework;

namespace Combat {
    public struct LimitedPriority : ICheckableData {
        public enum ModifierType {
            Constant,
            Follow,
            PercentFollow,
        }
        
        private Priority maxValue;
        private FloatF curValue;
        
        private SafeEvent<FloatF, FloatF> onValueChanged;
        public event Action<FloatF, FloatF> OnValueChanged {
            add {
                value?.Invoke(curValue, maxValue);
                onValueChanged?.Register(value);
            }
            remove { onValueChanged?.UnRegister(value); }
        }

        public LimitedPriority(FloatF maxValue) : this(maxValue, maxValue) {
        }
        
        public LimitedPriority(FloatF maxValue, FloatF value) {
            this.maxValue = new Priority(maxValue);
            curValue = value;
            onValueChanged = new SafeEvent<FloatF, FloatF>();
            RefreshValue();
        }

        public FloatF Value => curValue;
        public static implicit operator FloatF(LimitedPriority p) => p.Value;

        public void AddModifier(Priority.ModifierType type, FloatF value, ModifierType limitedType) {
            FloatF lastMaxValue = maxValue;
            maxValue.AddModifier(type, value);
            if (limitedType == ModifierType.Constant) {
            } else if (limitedType == ModifierType.PercentFollow) {
                curValue = maxValue * (curValue / lastMaxValue);
            } else if (limitedType == ModifierType.Follow) {
                curValue = curValue + (maxValue - lastMaxValue);
            }
            RefreshValue();
        }
        
        public void RemoveModifier(Priority.ModifierType type, FloatF value, ModifierType limitedType) {
            FloatF lastMaxValue = maxValue;
            maxValue.RemoveModifier(type, value);
            if (limitedType == ModifierType.Constant) {
            } else if (limitedType == ModifierType.PercentFollow) {
                curValue = maxValue * (curValue / lastMaxValue);
            } else if (limitedType == ModifierType.Follow) {
                curValue = curValue + (maxValue - lastMaxValue);
            }
            RefreshValue();
        }
        
        private void RefreshValue() {
            curValue = FloatF.Clamp(curValue, FloatF.zero, maxValue);
            onValueChanged.Send(curValue, maxValue);
        }

        public static LimitedPriority operator+(LimitedPriority p, FloatF value) {
            LimitedPriority ret = p;
            ret.curValue += value;
            ret.RefreshValue();
            return ret;
        }
        
        public static LimitedPriority operator-(LimitedPriority p, FloatF value) {
            LimitedPriority ret = p;
            ret.curValue -= value;
            ret.RefreshValue();
            return ret;
        }

        public int GetStatusCode() {
            int code = StatusCode.Seed;
            code = StatusCode.CombineData(code, maxValue);
            return StatusCode.Combine(code, curValue);
        }
    }
}
