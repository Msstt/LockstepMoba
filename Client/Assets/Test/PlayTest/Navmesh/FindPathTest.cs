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
            GameMgr.Instance.Init(new HashSet<Type> { typeof(INavmesh) });
            GameMgr.Instance.Start();
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
                        NavmeshUtils.FindPath(Vector3F.FromVector3(point.Value), Vector3F.FromVector3(hit.point), (path) => {
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
