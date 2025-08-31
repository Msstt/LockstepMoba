using UnityEngine;

public class Test : MonoBehaviour {
    private void Start() {
        Framework.Network.Network.Instance.Connect("192.168.101.155", 9980);
    }

    public void OnClick1() {
        Framework.Network.Network.Instance.Connect("192.168.101.155", 9980);
    }
    
    public void OnClick2() {
        Framework.Network.Network.Instance.DisConnect();
    }
}