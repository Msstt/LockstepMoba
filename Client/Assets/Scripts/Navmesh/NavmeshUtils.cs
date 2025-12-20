using System;
using System.Collections.Generic;
using Navmesh;

public static class NavmeshUtils {
    public static NavmeshMapInfo Config => GameMgr.Instance.GetSystem<INavmesh>().MapInfo;
    
    public static bool Raycast(Vector3F point, out int tId) {
        return GameMgr.Instance.GetSystem<INavmesh>().Raycast(0, point, out tId);
    }
    
    public static bool RaycastByRadius(FloatF radius, Vector3F point, out int tId) {
        return GameMgr.Instance.GetSystem<INavmesh>().Raycast(radius, point, out tId);
    }
    
    public static void FindPath(Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force = false) {
        GameMgr.Instance.GetSystem<INavmesh>().FindPath(0, start, end, callback, force);
    }
    
    public static void FindPathByRadius(FloatF radius, Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force = false) {
        GameMgr.Instance.GetSystem<INavmesh>().FindPath(radius, start, end, callback, force);
    }
}
