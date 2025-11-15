using Network;

public class TestMsgDispatcher : MsgDispatcher {
    public static void Register() {
        dispatcher.RegisterMsgHandler<echo_test_c2s>(MessageDef.echo_test_c2s, echo_test_c2s);
    }
    
    [Message(MessageDef.echo_test_c2s)]
    private static void echo_test_c2s(Uid uid, echo_test_c2s msg) {
        echo_test_s2c ret = new echo_test_s2c() {
            Id = msg.Id,
            Name = msg.Name,
        };
        NetworkUtils.Send(uid, MessageDef.echo_test_s2c, ret);
    }
}
