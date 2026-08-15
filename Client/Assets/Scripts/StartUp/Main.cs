using System;
using Combat.Actor;
using Framework;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UI;
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

        EventMgr.Instance.Register<EventType.OnGameStart>(() => {
            UIUtils.ShowUI(UIDef.SkillPanel);
            UIUtils.ShowUI(UIDef.LevelPanel);
                
            // GameMgr.Instance.GetSystem<IActorSystem>().CreateActor(new CreateMinion(1001, CampType.Blue, 0));
            // GameMgr.Instance.GetSystem<IActorSystem>().CreateActor(new CreateMinion(1001, CampType.Red, 0));
            //
            // var actor = ActorUtils.GetActor(2);
            // actor?.GetComponent<BuffCom>()?.AddBuff(4, 2, 1);
            // actor = ActorUtils.GetActor(3);
            // actor?.GetComponent<BuffCom>()?.AddBuff(4, 2, 1);
        });

        if (gmTool.IsLocalDebug) {
            GameMgr.Instance.StartLocalDebug();
        }
    }

    private Vector3? point = null;

    public void Update() {
        GameMgr.Instance.Update();
        
        // DebugUtils.DrawDot(GameMgr.Instance.GetSystem<IActorSystem>().SelfChampion.Pos);
    }

    public void OnApplicationQuit() {
        GameMgr.Instance.Quit();
    }
}