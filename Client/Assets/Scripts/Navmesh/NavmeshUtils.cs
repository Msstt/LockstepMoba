using System;
using System.Collections.Generic;
using Framework;
using Navmesh;
using UnityEditor.IMGUI.Controls;

public static class NavmeshUtils {
    public static NavmeshMapInfo Config => GameMgr.Instance.GetSystem<INavmesh>()?.MapInfo;
    
    public static void FindPath(Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force = false) {
        GameMgr.Instance.GetSystem<INavmesh>()?.FindPath(0, start, end, callback, force);
    }
    
    public static void FindPathByRadius(FloatF radius, Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force = false) {
        GameMgr.Instance.GetSystem<INavmesh>()?.FindPath(radius, start, end, callback, force);
    }
    
    public static bool IsReachable(Vector3F point) {
        return GameMgr.Instance.GetSystem<INavmesh>()?.IsReachable(0, point) ?? false;
    }
    
    public static bool IsReachableByRadius(FloatF radius, Vector3F point) {
        return GameMgr.Instance.GetSystem<INavmesh>()?.IsReachable(radius, point) ?? false;
    }
    
    public static Vector3F RaycastInSurface(Vector3F start, Vector3F end) {
        return GameMgr.Instance.GetSystem<INavmesh>()?.RaycastInSurface(0, start, end) ?? start;
    }
    
    public static Vector3F RaycastInSurface(FloatF radius, Vector3F start, Vector3F end) {
        return GameMgr.Instance.GetSystem<INavmesh>()?.RaycastInSurface(radius, start, end) ?? start;
    }


    public static void RegisterUnit(int id, int type, Vector3F pos, SafeEvent<Vector3F> onPosChange) {
        GameMgr.Instance.GetSystem<INavmesh>()?.RegisterUnit(id, type, pos, onPosChange);
    }

    public static void UnRegisterUnit(int id, SafeEvent<Vector3F> onPosChange) {
        GameMgr.Instance.GetSystem<INavmesh>()?.UnRegisterUnit(id, onPosChange);
        
    }

    public static List<int> RaycastInCircle(int type, Vector3F center, FloatF radius) {
        return GameMgr.Instance.GetSystem<INavmesh>()?.RaycastInCircle(1 << type, center, radius) ?? new List<int>();
    }
    
    private static int allTypeBitSet = (1 << UnitRaycaster.MaxTypeCount) - 1;
    public static List<int> RaycastInCircle(Vector3F center, FloatF radius) {
        return GameMgr.Instance.GetSystem<INavmesh>()?.RaycastInCircle(allTypeBitSet, center, radius) ?? new List<int>();
    }
}
