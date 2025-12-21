using Combat.Actor;
using Sirenix.OdinInspector;
using UnityEngine;

public class Main : MonoBehaviour {
    [LabelText("本地调试模式")]
    public bool IsLocalDebug = false;

    public void Awake() {
        GameMgr.Instance.Init();
    }

    public void Start() {
        GameMgr.Instance.Start();

        if (IsLocalDebug) {
            GameMgr.Instance.StartLocalDebug();
        }

        GameMgr.Instance.GetSystem<IActorSystem>().SelfChampion.Stats.MoveSpeed = 30;
    }

    private Vector3? point = null;

    public void Update() {
        GameMgr.Instance.Update();
        
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Map"))) {
                GameMgr.Instance.GetSystem<IActorSystem>().SelfChampion.GetComponent<MoveCom>().MoveToPos(Vector3F.FromVector3(hit.point));
            }
        }
    }
}