using System.Collections.Generic;
using Navmesh;

public static class NavmeshUtils {
    public static NavmeshMapInfo Config => NavmeshMgr.Instance.MapInfo;
    
    public static void Start() {
        NavmeshMgr.Instance.Start();
    }
    
    public static bool Raycast(Vector3F point, out int tId) {
        return NavmeshMgr.Instance.Raycast(0, point, out tId);
    }
    
    public static bool RaycastByRadius(FloatF radius, Vector3F point, out int tId) {
        return NavmeshMgr.Instance.Raycast(radius, point, out tId);
    }
    
    public static bool FindPath(Vector3F start, Vector3F end, out List<Vector3F> path) {
        return NavmeshMgr.Instance.FindPath(0, start, end, out path);
    }
    
    public static bool FindPathByRadius(FloatF radius, Vector3F start, Vector3F end, out List<Vector3F> path) {
        return NavmeshMgr.Instance.FindPath(radius, start, end, out path);
    }
}
