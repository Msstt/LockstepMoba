using Data;

public static class DataUtils {
    public static T Get<T>() where T : class, IData, new() {
        return GameMgr.Instance.GetSystem<IDataSystem>()?.Get<T>();
    }
}
