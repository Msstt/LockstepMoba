using System;

public static class EventUtils {
    public static void Register(EventDef eventDef, Action handler) {
        Framework.EventMgr.Instance.Register(eventDef, handler);
    }
    
    public static void Remove(EventDef eventDef, Action handler) {
        Framework.EventMgr.Instance.Remove(eventDef, handler);
    }
    
    public static void Register<T1>(EventDef eventDef, Action<T1> handler) {
        Framework.EventMgr.Instance.Register(eventDef, handler);
    }
    
    public static void Remove<T1>(EventDef eventDef, Action<T1> handler) {
        Framework.EventMgr.Instance.Remove(eventDef, handler);
    }
    
    public static void Register<T1, T2>(EventDef eventDef, Action<T1, T2> handler) {
        Framework.EventMgr.Instance.Register(eventDef, handler);
    }

    
    public static void Remove<T1, T2>(EventDef eventDef, Action<T1, T2> handler) {
        Framework.EventMgr.Instance.Remove(eventDef, handler);
    }
    
    public static void Send(EventDef eventDef, params object[] args) {
        Framework.EventMgr.Instance.Send(eventDef, args);
    }
}