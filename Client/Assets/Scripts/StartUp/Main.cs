using Combat.Actor;
using Sirenix.OdinInspector;
using UnityEngine;

public class Main : MonoBehaviour {
    [LabelText("本地调试模式")]
    public bool IsLocalDebug = false;

    public void Awake() {
        GameMgr.Instance.RegisterSystem();
    }

    public void Start() {
        GameMgr.Instance.Init();

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
                MoveCom com = GameMgr.Instance.GetSystem<IActorSystem>().SelfChampion.GetComponent<MoveCom>();
                com.ForceFail();
                com.MoveToPosByPath(Vector3F.FromVector3(hit.point),
                    () => {
                        Debug.Log("Move finished");
                    },
                    () => {
                        Debug.Log("Move failed");
                    });
            }
        }
        
        // DebugUtils.DrawDot(GameMgr.Instance.GetSystem<IActorSystem>().SelfChampion.Pos);
    }
}