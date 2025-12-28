using System;

namespace Combat.Actor {
    public class EventHub {
        public class SafeEvent {
            private Action listeners = () => { };

            public void Register(Action listener) => listeners += listener;
            public void UnRegister(Action listener) => listeners -= listener;
            public void Send() => listeners();
        }
        
        public readonly SafeEvent OnChangePos = new SafeEvent();
    };
}