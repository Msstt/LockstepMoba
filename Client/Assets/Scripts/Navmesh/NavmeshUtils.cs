using System;
using System.Collections.Generic;
using Navmesh;

public static class NavmeshUtils {
    public static NavmeshMapInfo Config => NavmeshMgr.Instance.MapInfo;
    
    public static void Start() {
        NavmeshMgr.Instance.Start();
    }
    
    public static void Update() {
        NavmeshMgr.Instance.Update();
    }
    
    public static bool Raycast(Vector3F point, out int tId) {
        return NavmeshMgr.Instance.Raycast(0, point, out tId);
    }
    
    public static bool RaycastByRadius(FloatF radius, Vector3F point, out int tId) {
        return NavmeshMgr.Instance.Raycast(radius, point, out tId);
    }
    
    public static void FindPath(Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force = false) {
        NavmeshMgr.Instance.FindPath(0, start, end, callback, force);
    }
    
    public static void FindPathByRadius(FloatF radius, Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force = false) {
        NavmeshMgr.Instance.FindPath(radius, start, end, callback, force);
    }
}
