using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindPathTest : SceneTest {
    protected override HashSet<Type> TestSystem => new() { typeof(Navmesh.INavmesh) };
    protected override string TestSceneName => "NavmeshTest";
    
    protected override void BeforeSceneLoad() {
        Navmesh.FindPathConfig.FindPathMaxIterationCount = 1000;
    }
    
    Vector3? point;

    protected override void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
                if (point != null) {
                    Debug.Log($"{point.Value} -> {hit.point}");
                    Vector3 start = point.Value;
                    Vector3 end = hit.point;
                    // Vector3 start = new Vector3(21.42f, 5.00f, 11.82f);
                    // Vector3 end = new Vector3(19.58f, 5.00f, 17.83f);
                    NavmeshUtils.FindPath(start.ToVector3F(), end.ToVector3F(), (path) => {
                        Debug.Log($"path Count: {path.Count}");
                        for (int i = 0; i + 1 < path.Count; i++) {
                            DebugUtils.DrawLine(path[i], path[i + 1], Color.red, 2, 0.05f);
                        }
                    });
                        
                    NavMeshPath sysPath = new NavMeshPath();
                    bool hasPath = NavMesh.CalculatePath(point.Value, hit.point, NavMesh.AllAreas, sysPath);
                    if (hasPath) {
                        for (int i = 0; i + 1 < sysPath.corners.Length; i++) {
                            DebugUtils.DrawLine(sysPath.corners[i], sysPath.corners[i + 1], Color.blue, 2, 0.05f);
                        }
                    }
                        
                    point = null;
                } else {
                    point = hit.point;
                }
            }
        }
    }
}
