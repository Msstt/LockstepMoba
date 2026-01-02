using Combat.Actor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

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
    }

    private Vector3? point = null;

    public void Update() {
        GameMgr.Instance.Update();
        
        // DebugUtils.DrawDot(GameMgr.Instance.GetSystem<IActorSystem>().SelfChampion.Pos);
    }
}