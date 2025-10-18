using Network;
using UnityEngine;

public static class TestMsgDispatcher {
    [Message(MessageDef.echo_test_s2c)]
    public static void echo_test_s2c(echo_test_s2c msg) {
        Debug.Log("echo_test receive: " + msg);
    }
}