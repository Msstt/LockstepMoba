using Network;

public static class FrameMsgDispatcher {
    [Message(MessageDef.frame_input_c2s)]
    public static void frame_input_c2s(Uid uid, frame_input_c2s msg) {
        LockStep.Instance.AddInput(msg.Frame, uid, msg.Input);
    }
    
    [Message(MessageDef.frame_reconnect_c2s)]
    public static void frame_reconnect_c2s(Uid uid, frame_reconnect_c2s msg) {
        int frame = msg.Frame;
        int maxFrame = LockStep.Instance.Frame;
        if (frame > maxFrame) {
            return;
        }
        void Send() {
            NetworkUtils.Send(uid, MessageDef.frame_input_s2c, LockStep.Instance.GetInputMsg(frame));
            frame++;
            if (frame <= maxFrame) {
                AsyncUtils.WaitFrameEnd(Send);
            }
        }
        AsyncUtils.WaitFrameEnd(Send);
    }
}