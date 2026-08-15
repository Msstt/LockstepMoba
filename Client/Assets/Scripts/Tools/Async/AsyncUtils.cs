using System;
using Framework;

public static class AsyncUtils {
    public static ReleaseToken Start(FloatF time, Action func) {
        return AsyncMgr.Instance.Start(TimeUtils.GetFrame(time), func);
    }
}
