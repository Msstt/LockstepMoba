using System;
using System.Collections;
using System.Collections.Generic;
using Navmesh;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FindPathTest {
    [SetUp]
    public void Setup() {
        SceneManager.LoadScene("NavmeshTest", LoadSceneMode.Single);
        SceneManager.sceneLoaded += (scene, mode) => {
            GameMgr.Instance.RegisterSystem(new HashSet<Type> { typeof(INavmesh) });
            GameMgr.Instance.Init();
        };
    }
    
    [UnityTest]
    public IEnumerator EchoTest1() {
        Vector3? point = null;
        while (true) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
                    if (point != null) {
                        Debug.Log($"{point.Value} -> {hit.point}");
                        Vector3 start = point.Value;
                        Vector3 end = hit.point;
                        // Vector3 start = new Vector3(21.42f, 5.00f, 11.82f);
                        // Vector3 end = new Vector3(19.58f, 5.00f, 17.83f);
                        NavmeshUtils.FindPath(Vector3F.FromVector3(start), Vector3F.FromVector3(end), (path) => {
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
            
            GameMgr.Instance.FrameUpdate();
            
            yield return null;
        }
    }
}
