using System;

public static class EventUtils {
    public static void Register<T>(Action<T> listener) where T : struct {
        Framework.EventMgr.Instance.Register(listener);
    }
    
    public static void UnRegister<T>(Action<T> listener) where T : struct {
        Framework.EventMgr.Instance.UnRegister(listener);
    }
    
    public static void Send<T>(T param) where T : struct {
        Framework.EventMgr.Instance.Send(param);
    }
    
    public static void Register<T>(Action listener) where T : struct {
        Framework.EventMgr.Instance.Register<T>(listener);
    }
    
    public static void UnRegister<T>(Action listener) where T : struct {
        Framework.EventMgr.Instance.UnRegister<T>(listener);
    }
    
    public static void Send<T>() where T : struct {
        Framework.EventMgr.Instance.Send(new T());
    }
}