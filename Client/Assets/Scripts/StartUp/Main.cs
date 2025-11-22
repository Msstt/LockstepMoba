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
                    NavmeshUtils.FindPath(Vector3F.FromVector3(point.Value), Vector3F.FromVector3(hit.point), out _);
                    point = null;
                } else {
                    point = hit.point;
                }
            }
        }
    }
}