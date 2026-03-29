using System;
using Framework;

namespace Combat.Actor {
    public class EventHub {
        public SafeEvent<Vector3F> OnChangePos { get; private set; } = new SafeEvent<Vector3F>();
        public SafeEvent<Damage> OnHit { get; private set; } = new SafeEvent<Damage>();
    };
}