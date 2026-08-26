using System;
using Sirenix.OdinInspector;

namespace Combat.Actor {
    // 注册到 UnitRaycaster 的 type
    [Flags] [Serializable]
    [DrawWithUnity]
    public enum ActorType {
        Champion = 1 << 0,
        Minion = 1 << 1,
        Turret = 1 << 2,
    }
}