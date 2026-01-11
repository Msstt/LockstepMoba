using System;
using System.Collections.Generic;
using Navmesh;

public static class NavmeshUtils {
    public static NavmeshMapInfo Config => GameMgr.Instance.GetSystem<INavmesh>()?.MapInfo;
    
    public static bool IsReachable(Vector3F point) {
        return GameMgr.Instance.GetSystem<INavmesh>()?.IsReachable(0, point) ?? false;
    }
    
    public static bool IsReachableByRadius(FloatF radius, Vector3F point) {
        return GameMgr.Instance.GetSystem<INavmesh>()?.IsReachable(radius, point) ?? false;
    }
    
    public static void FindPath(Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force = false) {
        GameMgr.Instance.GetSystem<INavmesh>()?.FindPath(0, start, end, callback, force);
    }
    
    public static void FindPathByRadius(FloatF radius, Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force = false) {
        GameMgr.Instance.GetSystem<INavmesh>()?.FindPath(radius, start, end, callback, force);
    }
}
