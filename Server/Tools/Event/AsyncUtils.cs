
public static class AsyncUtils {
    public static void WaitFrameEnd(Action callback) {
        Framework.AsyncMgr.Instance.WaitFrameEnd(callback);
    }
    
    public static void Update() {
        Framework.AsyncMgr.Instance.Update();
    }
}