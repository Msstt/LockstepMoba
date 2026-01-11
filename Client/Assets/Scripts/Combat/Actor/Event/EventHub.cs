using System;
using Framework;

namespace Combat.Actor {
    public class EventHub {
        public readonly SafeEvent OnChangePos = new SafeEvent();
    };
}