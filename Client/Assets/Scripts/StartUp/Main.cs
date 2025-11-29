using UnityEngine;

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
                    // NavmeshUtils.FindPath(Vector3F.FromVector3(new Vector3(10.31f, 5.00f, 8.70f)), Vector3F.FromVector3(new Vector3(10.75f, 5.00f, 10.20f)), out var path);
                    for (int i = 0; i + 1 < path.Count; i++) {
                        DebugUtils.DrawLine(path[i], path[i + 1]);
                    }
                    point = null;
                } else {
                    point = hit.point;
                }
            }
        }
    }
}