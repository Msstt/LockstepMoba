using Framework;
using Framework.Network;
using Network;

public static class TestMsgDispatcher {
    [Message(MessageDef.echo_test_c2s)]
    public static void echo_test_c2s(Uid uid, echo_test_c2s msg) {
        echo_test_s2c ret = new echo_test_s2c() {
            Id = msg.Id,
            Name = msg.Name,
        };
        NetworkUtils.Send(uid, MessageDef.echo_test_s2c, ret);
    }
}
