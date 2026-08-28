using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat {
    public struct Priority : ICheckableData {
        [DrawWithUnity]
        public enum ModifierType {
            [InspectorName("+")]
            Add,
            [InspectorName("+%")]
            PercentAdd,
            [InspectorName("*")]
            PercentMult,
        }
        
        private static readonly FloatF f100 = new FloatF(100);
        
        private readonly FloatF baseValue;
        private FloatF finalValue;
        private FloatF addValue;
        private FloatF addPercent;
        private FloatF multValue;
        
        private SafeEvent<FloatF> onValueChanged;
        public event Action<FloatF> OnValueChanged {
            add {
                value?.Invoke(finalValue);
                onValueChanged?.Register(value);
            }
            remove { onValueChanged?.UnRegister(value); }
        }
        
        public Priority(FloatF baseValue, bool noEvent = false) {
            this.baseValue = baseValue;
            finalValue = baseValue;
            addValue = FloatF.zero;
            addPercent = FloatF.zero;
            multValue = FloatF.one;
            onValueChanged = noEvent ? new SafeEvent<FloatF>() : null;
        }
        
        public FloatF Value => finalValue;
        public static implicit operator FloatF(Priority p) => p.Value;

        public void AddModifier(ModifierType type, FloatF value) {
            if (type == ModifierType.Add) {
                addValue += value;
            } else if (type == ModifierType.PercentAdd) {
                addPercent += value;
            } else if (type == ModifierType.PercentMult) {
                multValue *= (FloatF.one + value / f100);
            }
            RefreshValue();
        }
        
        public void RemoveModifier(ModifierType type, FloatF value) {
            if (type == ModifierType.Add) {
                addValue -= value;
            } else if (type == ModifierType.PercentAdd) {
                addPercent -= value;
            } else if (type == ModifierType.PercentMult) {
                multValue /= (FloatF.one + value / f100);
            }
            RefreshValue();
        }

        private void RefreshValue() {
            finalValue = (baseValue + addValue) * (1 + addPercent / f100) * multValue;
            onValueChanged?.Send(finalValue);
        }

        public int GetStatusCode() {
            int code = StatusCode.Seed;
            code = StatusCode.Combine(code, baseValue);
            code = StatusCode.Combine(code, finalValue);
            code = StatusCode.Combine(code, addValue);
            code = StatusCode.Combine(code, addPercent);
            return StatusCode.Combine(code, multValue);
        }
    }
}
