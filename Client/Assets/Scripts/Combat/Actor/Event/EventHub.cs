using System;
using Framework;

namespace Combat.Actor {
    public class EventHub {
        public SafeEvent OnChangePos { get; private set; } = new SafeEvent();
    };
}