using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class UnitRaycastTest : SceneTest {
    protected override HashSet<Type> TestSystem => new() { typeof(Navmesh.INavmesh) };
    protected override string TestSceneName => "NavmeshTest";
    
    protected override void BeforeSceneLoad() {
        Navmesh.FindPathConfig.FindPathMaxIterationCount = 1000;
    }

    private int id = 0;
    private List<Vector3F> dot = new List<Vector3F>();
    private SafeEvent <Vector3F> onPosChange = new SafeEvent<Vector3F>();
    
    protected override void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
                Vector3F pos = hit.point.ToVector3F();
                NavmeshUtils.RegisterUnit(++id, 0, pos, onPosChange);
                dot.Add(pos);
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            if (id > 0) {
                NavmeshUtils.UnRegisterUnit(id--, onPosChange);
                dot.RemoveAt(dot.Count - 1);
            }
        }
        
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
                DebugUtils.DrawCircle(hit.point, 3, 0.05f);
                Vector3F pos = hit.point.ToVector3F();
                using (PooledList<int> result = PooledList<int>.Get()) {
                    NavmeshUtils.RaycastInCircle(pos, 3, result);
                    Debug.Log(result.Count);
                    foreach (int unitId in result) {
                        Debug.Log($"unitId: {unitId}");
                        DebugUtils.DrawDot(dot[unitId - 1], Color.green, 2, 0.1f);
                    }
                }
            }
        }
    }
}
