using System;
using System.Collections.Generic;
using Framework.Network;
using Network;
using UnityEngine;
using UnityEngine.UI;

public class LockstepTest : SceneTest {
    protected override HashSet<Type> TestSystem => new() { typeof(INetwork), typeof(ILockStep) };
    protected override string TestSceneName => "LockstepTest";
    
    private int count = 0;
    private Dictionary<Uid, int> inputCount = new Dictionary<Uid, int>();
    private Text statusText;

    protected override void AfterSceneLoad() {
        EventUtils.Register<EventType.OnLockStepStart>(OnLockStepStart);
        GameObject.Find("Canvas/+1").GetComponent<Button>().onClick.AddListener(() => {
            count = 1;
        });
        GameObject.Find("Canvas/connect").GetComponent<Button>().onClick.AddListener(() => {
            GameMgr.Instance.GetSystem<INetwork>().Connect("127.0.0.1", 9980);
        });
        GameObject.Find("Canvas/disconnect").GetComponent<Button>().onClick.AddListener(() => {
            GameMgr.Instance.GetSystem<INetwork>().Disconnect();
        });
        statusText = GameObject.Find("Canvas/status").GetComponent<Text>();
    }

    private void OnLockStepStart() {
        NetworkUtils.RegisterCollector<test_input>(MessageDef.test_input, TestInputCollector);
        NetworkUtils.RegisterHandler<test_input>(MessageDef.test_input, TestInputHandler);
    }
    
    private test_input TestInputCollector() {
        var ret = new test_input {
            Count = count,
        };
        count = 0;
        return ret;
    }
    
    private void TestInputHandler(SortedDictionary<Uid, test_input> inputs) {
        string status = "";
        foreach (var (uid, tMsg) in inputs) {
            var msg = tMsg;
            if (!inputCount.ContainsKey(uid)) {
                inputCount[uid] = 0;
            }
            inputCount[uid] += msg.Count;
        }
        foreach (var (uid, value) in inputCount) {
            status += uid + ": " + value + "\n";
        }
        statusText.text = status;
    }
}