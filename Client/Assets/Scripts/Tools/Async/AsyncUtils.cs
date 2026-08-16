using System;
using Framework;

public static class AsyncUtils {
    public static ReleaseToken StartSecond(FloatF time, Action func) {
        return AsyncMgr.Instance.Start(TimeUtils.GetFrame(time), func);
    }
    
    public static ReleaseToken StartFrame(int frame, Action func) {
        return AsyncMgr.Instance.Start(GameMgr.Instance.Frame + frame, func);
    }
    
    public static ReleaseToken StartEndFrame(Action func) {
        return AsyncMgr.Instance.Start(GameMgr.Instance.Frame, func);
    }
}
