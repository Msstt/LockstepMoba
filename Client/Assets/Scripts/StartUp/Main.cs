using Combat.Skill;
using Framework;
using UnityEngine;

public class Main : MonoBehaviour {
    private GMTool gmTool;

    public void Awake() {
        gmTool = GetComponent<GMTool>();
        GameMgr.Instance.GMTool = gmTool;
        GameMgr.Instance.RegisterSystem();
    }

    public void Start() {
        GameMgr.Instance.Init();

        if (gmTool.IsLocalDebug) {
            GameMgr.Instance.StartLocalDebug();
        }
    }

    private Vector3? point = null;

    public void Update() {
        GameMgr.Instance.Update();
        
        // DebugUtils.DrawDot(GameMgr.Instance.GetSystem<IActorSystem>().SelfChampion.Pos);
    }
}