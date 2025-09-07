using Network;
using UnityEngine;

public class Test : MonoBehaviour {
    private void Start() {
        Framework.Network.Network.Instance.Connect("192.168.101.155", 9980);
        Framework.Network.MsgDispatcher.Instance.RegisterHandler(MessageDef.Test, (msg) => {
            Debug.Log("M: " + msg);
            throw new System.Exception("Test Exception");
        });
    }

    public void OnClick1() {
        Framework.Network.Network.Instance.Connect("192.168.101.155", 9980);
    }
    
    public void OnClick2() {
        Framework.Network.Network.Instance.Disconnect();
    }

    private int x = 1;
    public void Update() {
        var msg = new Framework.Network.Message();
        msg.msgId = MessageDef.Test;
        msg.data = new TestMsg() {
            Id = x,
            Name = "Test" + x,
        };
        x++;
        Framework.Network.Network.Instance.Send(msg);
    }
}