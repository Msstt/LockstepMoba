using UnityEngine;
using UnityEngine.AI;

public class Main : MonoBehaviour {
    public void Start() {
        GameMgr.Instance.Start();
    }

    private Vector3? point = null;

    public void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
                if (point != null) {
                    Debug.Log(point.Value);
                    Debug.Log(hit.point);
                    NavmeshUtils.FindPath(Vector3F.FromVector3(point.Value), Vector3F.FromVector3(hit.point), out var path);
                    // Vector3 s = new Vector3(677.11f, 125.01f, 1490.28f);
                    // Vector3 e = new Vector3(591.55f, 125.01f, 1456.38f);
                    // DebugUtils.DrawDot(s);
                    // DebugUtils.DrawDot(e);
                    // NavmeshUtils.FindPath(Vector3F.FromVector3(s), Vector3F.FromVector3(e), out var path);
                    for (int i = 0; i + 1 < path.Count; i++) {
                        DebugUtils.DrawLine(path[i], path[i + 1], Color.red);
                    }
                    Debug.Log($"path Count: {path.Count}");
                    
                    NavMeshPath ppath = new NavMeshPath();
                    bool hasPath = NavMesh.CalculatePath(point.Value, hit.point, NavMesh.AllAreas, ppath);
                    
                    if (hasPath)
                    {
                        // path.corners 就是路径点数组
                        for (int i = 0; i + 1 < ppath.corners.Length; i++)
                        {
                            DebugUtils.DrawLine(ppath.corners[i], ppath.corners[i + 1], Color.blue);
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