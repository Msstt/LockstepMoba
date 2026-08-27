using System;
using System.Collections.Generic;
using Combat.Actor;
using UnityEngine;

public class ObstacleAvoidTest : SceneTest {
    protected override HashSet<Type> TestSystem => new() { typeof(Navmesh.INavmesh), typeof(IActorSystem) };
    protected override string TestSceneName => "NavmeshTest";
    
    protected override void AfterSceneLoad() {
        GameObject config = GameObject.Find("Config");
        var actorSystem = GameMgr.Instance.GetSystem<IActorSystem>();
        int count = config.transform.childCount;
        for (int i = 0; i < count; i++) {
            var actor = actorSystem.CreateActor(new CreateTestUnit(config.transform.GetChild(i)));
            actor.GetComponent<MoveCom>()
                .MoveToPosByPath(config.transform.GetChild((i + count / 2) % count).position.ToVector3F());
        }
    }
}