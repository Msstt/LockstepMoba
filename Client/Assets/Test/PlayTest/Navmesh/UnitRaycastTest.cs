using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Navmesh;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class UnitRaycastTest {
    [SetUp]
    public void Setup() {
        Navmesh.FindPathConfig.FindPathMaxIterationCount = 1000;
        SceneManager.LoadScene("NavmeshTest", LoadSceneMode.Single);
        SceneManager.sceneLoaded += (scene, mode) => {
            GameMgr.Instance.RegisterSystem(new HashSet<Type> { typeof(Navmesh.INavmesh) });
            GameMgr.Instance.Init();
        };
    }

    private int id = 0;
    private List<Vector3F> dot = new List<Vector3F>();
    
    [UnityTest]
    public IEnumerator Test1() {
        SafeEvent <Vector3F> onPosChange = new SafeEvent<Vector3F>();
        while (true) {
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
                    List<int> result = NavmeshUtils.RaycastInCircle(pos, 3);
                    Debug.Log(result.Count);
                    foreach (int unitId in result) {
                        Debug.Log($"unitId: {unitId}");
                        DebugUtils.DrawDot(dot[unitId - 1], Color.green, 2, 0.1f);
                    }
                }
            }
            
            
            GameMgr.Instance.FrameUpdate();
            
            yield return null;
        }
    }
}
