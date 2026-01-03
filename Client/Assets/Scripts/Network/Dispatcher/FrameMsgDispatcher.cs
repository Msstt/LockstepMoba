using Network;

public class FrameMsgDispatcher : MsgDispatcher {
    public static void Register() {
        Register<frame_start_s2c>(MessageDef.frame_start_s2c, frame_start_s2c);
        Register<frame_input_s2c>(MessageDef.frame_input_s2c, frame_input_s2c);
    }
    
    private static void frame_start_s2c(frame_start_s2c msg) {
        // 客户端应该模拟到 msg.Frame - 1，但 frame_input_s2c 和 frame_start_s2c 可能会在同一帧收到，所以加了一帧的容错
        if (msg.Frame - 1 <= GameMgr.Instance.Frame + 1) {
            NetworkUtils.Send(MessageDef.frame_input_c2s, GameMgr.Instance.GetSystem<ILockStep>().GetInputMsg());
        }
    }
    
    private static void frame_input_s2c(frame_input_s2c msg) {
        GameMgr.Instance.GetSystem<ILockStep>().PushInputMsg(msg);
    }
}