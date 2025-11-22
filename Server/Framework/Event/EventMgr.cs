// 全局事件

namespace Framework {
    public class EventMgr : Singleton<EventMgr> {
        
        private Dictionary<EventDef, Delegate> eventHandlers = new Dictionary<EventDef, Delegate>();

        public EventMgr() {
            foreach (EventDef value in Enum.GetValues(typeof(EventDef))) {
                eventHandlers.Add(value, null);
            }
        }
        
        public void Register(EventDef eventDef, Delegate handler) {
            eventHandlers[eventDef] = Delegate.Combine(eventHandlers[eventDef], handler);
        }
        
        public void Remove(EventDef eventDef, Delegate handler) {
            eventHandlers[eventDef] = Delegate.Remove(eventHandlers[eventDef], handler);
        }
        
        public void Send(EventDef eventDef) {
            if (eventHandlers[eventDef] is not Action handler) {
                return;
            }
            try {
                handler?.Invoke();
            } catch (Exception e) {
                Console.WriteLine($"[EventMgr] Error calling {eventDef.ToString()}: {e}");
            }
        }

        public void Send<T1>(EventDef eventDef, T1 param1) {
            if (eventHandlers[eventDef] is not Action<T1> handler) {
                return;
            }
            try {
                handler?.Invoke(param1);
            } catch (Exception e) {
                Console.WriteLine($"[EventMgr] Error calling {eventDef.ToString()}: {e}");
            }
        }
        
        public void Send<T1, T2>(EventDef eventDef, T1 param1, T2 param2) {
            if (eventHandlers[eventDef] is not Action<T1, T2> handler) {
                return;
            }
            try {
                handler?.Invoke(param1, param2);
            } catch (Exception e) {
                Console.WriteLine($"[EventMgr] Error calling {eventDef.ToString()}: {e}");
            }
        }
    }
}