using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FogTest {
    [SetUp]
    public void Setup() {
        SceneManager.LoadScene("FogTest", LoadSceneMode.Single);
        SceneManager.sceneLoaded += (scene, mode) => {
            GameMgr.Instance.RegisterSystem(new HashSet<Type> { typeof(Navmesh.INavmesh), typeof(Combat.Fog.IFogSystem) });
            GameMgr.Instance.Init();
        };
    }
    
    [UnityTest]
    public IEnumerator Test1() {
        while (true) {
            GameMgr.Instance.FrameUpdate();
            GameMgr.Instance.Update();
            
            yield return null;
        }
    }
}
