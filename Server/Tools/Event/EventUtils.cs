
public static class EventUtils {
    public static void Register(EventDef eventDef, Action handler) {
        Framework.EventMgr.Instance.Register(eventDef, handler);
    }
    
    public static void Remove(EventDef eventDef, Action handler) {
        Framework.EventMgr.Instance.Remove(eventDef, handler);
    }
    
    public static void Send(EventDef eventDef) {
        Framework.EventMgr.Instance.Send(eventDef);
    }
    
    public static void Register<T1>(EventDef eventDef, Action<T1> handler) {
        Framework.EventMgr.Instance.Register(eventDef, handler);
    }
    
    public static void Remove<T1>(EventDef eventDef, Action<T1> handler) {
        Framework.EventMgr.Instance.Remove(eventDef, handler);
    }
    
    public static void Send<T1>(EventDef eventDef, T1 param1) {
        Framework.EventMgr.Instance.Send(eventDef, param1);
    }
    
    public static void Register<T1, T2>(EventDef eventDef, Action<T1, T2> handler) {
        Framework.EventMgr.Instance.Register(eventDef, handler);
    }

    
    public static void Remove<T1, T2>(EventDef eventDef, Action<T1, T2> handler) {
        Framework.EventMgr.Instance.Remove(eventDef, handler);
    }
    
    public static void Send<T1, T2>(EventDef eventDef, T1 param1, T2 param2) {
        Framework.EventMgr.Instance.Send(eventDef, param1, param2);
    }
}