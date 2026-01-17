using System;
using Framework;
using UnityEngine;

namespace Combat {
    public struct LimitedPriority {
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
                value?.Invoke(curValue, maxValue.Value);
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

        public void AddModifier(Priority.ModifierType type, FloatF value, ModifierType limitedType) {
            FloatF lastMaxValue = maxValue.Value;
            maxValue.AddModifier(type, value);
            if (limitedType == ModifierType.Constant) {
            } else if (limitedType == ModifierType.PercentFollow) {
                curValue = maxValue.Value * (curValue / lastMaxValue);
            } else if (limitedType == ModifierType.Follow) {
                curValue = curValue + (maxValue.Value - lastMaxValue);
            }
            RefreshValue();
        }
        
        public void RemoveModifier(Priority.ModifierType type, FloatF value, ModifierType limitedType) {
            FloatF lastMaxValue = maxValue.Value;
            maxValue.RemoveModifier(type, value);
            if (limitedType == ModifierType.Constant) {
            } else if (limitedType == ModifierType.PercentFollow) {
                curValue = maxValue.Value * (curValue / lastMaxValue);
            } else if (limitedType == ModifierType.Follow) {
                curValue = curValue + (maxValue.Value - lastMaxValue);
            }
            RefreshValue();
        }
        
        private void RefreshValue() {
            curValue = FloatF.Clamp(curValue, FloatF.zero, maxValue.Value);
            onValueChanged.Send(curValue, maxValue.Value);
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
    }
}