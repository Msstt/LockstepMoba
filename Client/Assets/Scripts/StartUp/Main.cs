using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class Main : MonoBehaviour {
    [LabelText("本地调试模式")]
    public bool IsLocalDebug = false;

    public FloatF test;

    public void Awake() {
        GameMgr.Instance.Init();
    }

    public void Start() {
        GameMgr.Instance.Start();

        if (IsLocalDebug) {
            GameMgr.Instance.StartLocalDebug();
        }
    }

    private Vector3? point = null;

    public void Update() {
        GameMgr.Instance.Update();
        
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
                if (point != null) {
                    Debug.Log(point.Value);
                    Debug.Log(hit.point);
                    Action<List<Vector3F>> finish = (path) => {
                        for (int i = 0; i + 1 < path.Count; i++) {
                            DebugUtils.DrawLine(path[i] + new Vector3F(0, 10, 0), path[i + 1] + new Vector3F(0, 10, 0), Color.red);
                        }

                        Debug.Log($"path Count: {path.Count}");
                    };
                    NavmeshUtils.FindPath(Vector3F.FromVector3(point.Value), Vector3F.FromVector3(hit.point), finish);
                    // Vector3 s = new Vector3(677.11f, 125.01f, 1490.28f);
                    // Vector3 e = new Vector3(591.55f, 125.01f, 1456.38f);
                    // DebugUtils.DrawDot(s);
                    // DebugUtils.DrawDot(e);
                    // NavmeshUtils.FindPath(Vector3F.FromVector3(s), Vector3F.FromVector3(e), out var path);
                    
                    
                    // NavMeshPath ppath = new NavMeshPath();
                    // bool hasPath = NavMesh.CalculatePath(point.Value, hit.point, NavMesh.AllAreas, ppath);
                    //
                    // if (hasPath)
                    // {
                    //     // path.corners 就是路径点数组
                    //     for (int i = 0; i + 1 < ppath.corners.Length; i++)
                    //     {
                    //         DebugUtils.DrawLine(ppath.corners[i], ppath.corners[i + 1], Color.blue);
                    //     }
                    // }
                    
                    point = null;
                } else {
                    point = hit.point;
                }
            }
        }
    }
}