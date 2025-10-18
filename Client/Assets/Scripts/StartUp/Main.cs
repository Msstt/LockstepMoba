using UnityEngine;

public class Main : MonoBehaviour {
    private void Start() {
        GameMgr.Instance.Start();
    }
    
    public void Update() {
        GameMgr.Instance.Update();
    }

    public void OnClick1() {
        Framework.Network.Network.Instance.Connect("127.0.0.1", 9980);
    }
    
    public void OnClick2() {
        Framework.Network.Network.Instance.Disconnect();
    }
}