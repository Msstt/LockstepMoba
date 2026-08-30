using System;
using System.Collections.Generic;
using Framework;
using Navmesh;

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


    public static void RegisterUnit(int id, int typeBitSet, Vector3F pos, SafeEvent<Vector3F> onPosChange) {
        for (int i = 0; i < UnitRaycaster.MaxTypeCount; i++) {
            if (((typeBitSet >> i) & 1) != 0) {
                GameMgr.Instance.GetSystem<INavmesh>()?.RegisterUnit(id, i, pos, onPosChange);
            }
        }
    }

    public static void UnRegisterUnit(int id, SafeEvent<Vector3F> onPosChange) {
        GameMgr.Instance.GetSystem<INavmesh>()?.UnRegisterUnit(id, onPosChange);
        
    }

    public static void RaycastInCircle(int typeBitSet, Vector3F center, FloatF radius, List<int> results) {
        GameMgr.Instance.GetSystem<INavmesh>()?.RaycastInCircle(typeBitSet, center, radius, results);
    }
    
    private static int allTypeBitSet = (1 << UnitRaycaster.MaxTypeCount) - 1;
    public static void RaycastInCircle(Vector3F center, FloatF radius, List<int> results) {
        RaycastInCircle(allTypeBitSet, center, radius, results);
    }
    
    public static void RaycastInRect(int typeBitSet, Vector3F center, Vector3F direction, FloatF length, FloatF width, List<int> results) {
        GameMgr.Instance.GetSystem<INavmesh>()?.RaycastInRect(typeBitSet, center, direction, length, width, results);
    }
    
    public static void RaycastInRect(Vector3F center, Vector3F direction, FloatF length, FloatF width, List<int> results) {
        RaycastInRect(allTypeBitSet, center, direction, length, width, results);
    }

    public static float GetHeight(float x, float y) {
        return GameMgr.Instance.GetSystem<INavmesh>()?.GetHeight(x, y) ?? 0;
    }
}
