using System;

public static class EventUtils {
    
    public static void Register(EventDef eventDef, Delegate handler) {
        Framework.EventMgr.Instance.Register(eventDef, handler);
    }
    
    public static void Remove(EventDef eventDef, Delegate handler) {
        Framework.EventMgr.Instance.Remove(eventDef, handler);
    }
    
    public static void Send(EventDef eventDef, params object[] args) {
        Framework.EventMgr.Instance.Send(eventDef, args);
    }
}