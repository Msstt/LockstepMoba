
public static class TimeUtils {
    public static FloatF DeltaTime => GameMgr.Instance.DeltaTime;
    
    public static int GetFrame(FloatF time) {
        if (time <= 0) {
            return GameMgr.Instance.Frame;
        }
        return GameMgr.Instance.Frame + FloatF.FloorInt(time / GameMgr.Instance.DeltaTime);
    }
}
