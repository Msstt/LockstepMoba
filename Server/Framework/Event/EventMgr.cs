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

        public void Send(EventDef eventDef, params object[] args) {
            foreach (var func in eventHandlers[eventDef].GetInvocationList()) {
                try {
                    func.DynamicInvoke(args);
                } catch (Exception e) {
                    Console.WriteLine($"[EventMgr] Error calling {eventDef.ToString()}: {e}");
                }
            }
        }
    }
}
