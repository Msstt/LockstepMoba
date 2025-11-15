using Network;

public class FrameMsgDispatcher : MsgDispatcher {
    public static void Register() {
        dispatcher.RegisterHandler<frame_start_s2c>(MessageDef.frame_start_s2c, frame_start_s2c);
        dispatcher.RegisterHandler<frame_input_s2c>(MessageDef.frame_input_s2c, frame_input_s2c);
    }
    
    private static void frame_start_s2c(frame_start_s2c msg) {
        // 延迟 5 帧，发给服务器也会被丢掉，暂时写死
        if (msg.Frame - (LockStep.Instance.Frame + 1) > 5) {
            return;
        }
        NetworkUtils.Send(MessageDef.frame_input_c2s, LockStep.Instance.GetInputMsg());
    }
    
    private static void frame_input_s2c(frame_input_s2c msg) {
        LockStep.Instance.PushInputMsg(msg);
    }
}