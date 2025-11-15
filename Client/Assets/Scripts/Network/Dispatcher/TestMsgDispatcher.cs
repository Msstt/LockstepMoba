using Network;
using UnityEngine;

public class TestMsgDispatcher : MsgDispatcher {
    public static void Register() {
        dispatcher.RegisterHandler<echo_test_s2c>(MessageDef.echo_test_s2c, echo_test_s2c);
    }
    
    private static void echo_test_s2c(echo_test_s2c msg) {
        Debug.Log("echo_test receive: " + msg);
    }
}