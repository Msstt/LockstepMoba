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
    public void InputTest() {
        if (!hasAdd) {
            LockStep.Instance.RegisterCollector(MessageDef.test_input, () => {
                return new test_input {
                    Count = 1,
                };
            });
            LockStep.Instance.RegisterHandler(MessageDef.test_input, (inputs) => {
                string status = "";
                foreach (var pair in inputs) {
                    test_input msg = pair.Value as test_input;
                    if (msg == null) {
                        continue;
                    }
                    if (!inputCount.ContainsKey(pair.Key)) {
                        inputCount[pair.Key] = 0;
                    }
                    inputCount[pair.Key] += msg.Count;
                }
                foreach (var pair in inputCount) {
                    status += pair.Key + ": " + pair.Value + "\n";
                }
                statusText.text = status;
            });
            hasAdd = true;
        }
    }

    #endregion
}