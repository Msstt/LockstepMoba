using Network;

public class FrameMsgDispatcher : MsgDispatcher {
    public static void Register() {
        Register<frame_start_s2c>(MessageDef.frame_start_s2c, frame_start_s2c);
        Register<frame_input_s2c>(MessageDef.frame_input_s2c, frame_input_s2c);
    }
    
    private static void frame_start_s2c(frame_start_s2c msg) {
        // 延迟 5 帧，发给服务器也会被丢掉，暂时写死
        if (msg.Frame - (GameMgr.Instance.Frame + 1) > 5) {
            return;
        }
        NetworkUtils.Send(MessageDef.frame_input_c2s, GameMgr.Instance.GetSystem<ILockStep>().GetInputMsg());
    }
    
    private static void frame_input_s2c(frame_input_s2c msg) {
        GameMgr.Instance.GetSystem<ILockStep>().PushInputMsg(msg);
    }
}