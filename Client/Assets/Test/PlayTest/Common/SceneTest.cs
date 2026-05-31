using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public abstract class SceneTest {
    protected virtual HashSet<Type> TestSystem { get; }
    protected virtual string TestSceneName { get; }
    
    protected virtual void BeforeSceneLoad() {
    }
    
    protected virtual void AfterSceneLoad() {
    }

    protected virtual void Update() {
    }
    
    [SetUp]
    public void Setup() {
        BeforeSceneLoad();
        SceneManager.LoadScene(TestSceneName, LoadSceneMode.Single);
        SceneManager.sceneLoaded += (scene, mode) => {
            GameMgr.Instance.RegisterSystem(TestSystem);
            GameMgr.Instance.Init();
        };
        AfterSceneLoad();
    }
    
    [UnityTest]
    public IEnumerator Test() {
        while (true) {
            Update();
            
            GameMgr.Instance.FrameUpdate();
            GameMgr.Instance.Update();
            
            yield return null;
        }
    }
    
    [TearDown]
    public void TearDown() {
        GameMgr.Instance.Quit();
    }
}
