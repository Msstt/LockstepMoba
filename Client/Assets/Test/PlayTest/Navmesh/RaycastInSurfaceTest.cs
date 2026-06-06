using System;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInSurfaceTest : SceneTest {
    protected override HashSet<Type> TestSystem => new() { typeof(Navmesh.INavmesh) };
    protected override string TestSceneName => "NavmeshTest";
    
    Vector3? point;

    protected override void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
                if (point != null) {
                    Debug.Log($"{point.Value} -> {hit.point}");
                    Vector3 start = point.Value;
                    Vector3 end = hit.point;
                    
                    // start = new Vector3(21.66f, 5.00f, 12.80f);
                    // end = new Vector3(22.09f, 6.03f, 14.96f);
                    
                    DebugUtils.DrawLine(start, end, Color.blue, 2, 0.05f);
                    Vector3F ret = NavmeshUtils.RaycastInSurface(start.ToVector3F(), end.ToVector3F());
                    DebugUtils.DrawDot(ret, Color.red, 2, 0.3f);
                    
                    point = null;
                } else {
                    point = hit.point;
                }
            }
        }
    }
}
