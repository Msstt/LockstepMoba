using Combat.Area;
using UnityEngine;

public static class AreaUtils {
    public static Transform TransRoot => GameMgr.Instance.GetSystem<IAreaSystem>()?.TransRoot ?? null;

    public static int CreateArea(int areaId, int actorUid, int level, Vector3F position, Vector3F direction, int? targetUid = null) {
        return GameMgr.Instance.GetSystem<IAreaSystem>()?.CreateArea(areaId, actorUid, level, position, direction, targetUid) ?? -1;
    }

    public static void DestroyArea(int uid) {
        GameMgr.Instance.GetSystem<IAreaSystem>()?.DestroyArea(uid);
    }
}
