using System.Collections.Generic;
using Battle;
using Network;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {
    private void Start() {
        GameMgr.Instance.Start();
    }
    
    // public void Update() {
    //     GameMgr.Instance.Update();
    // }

    #region 临时测试代码
    
    public void ConnectToServer() {
        Framework.Network.Network.Instance.Connect("127.0.0.1", 9980);
    }
    
    public void Disconnect() {
        Framework.Network.Network.Instance.Disconnect();
    }

    private bool hasAdd = false;
    private Dictionary<Uid, int> inputCount = new();
    public Text statusText;
    private int count = 0;
    public void InputTestStart() {
        if (!hasAdd) {
            LockStep.Instance.RegisterCollector(MessageDef.test_input, () => {
                var ret = new test_input {
                    Count = count,
                };
                count = 0;
                return ret;
            });
            LockStep.Instance.RegisterHandler(MessageDef.test_input, (inputs) => {
                string status = "";
                foreach (var (uid, tMsg) in inputs) {
                    var msg = tMsg as test_input;
                    if (msg == null) {
                        continue;
                    }
                    if (!inputCount.ContainsKey(uid)) {
                        inputCount[uid] = 0;
                    }
                    inputCount[uid] += msg.Count;
                }
                foreach (var (uid, count) in inputCount) {
                    status += uid + ": " + count + "\n";
                }
                statusText.text = status;
            });
            hasAdd = true;
        }
    }
    
    public void InputTest() {
        count = 1;
    }

    #endregion
}