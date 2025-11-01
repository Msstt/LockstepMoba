using Network;

public static class FrameMsgDispatcher {
    [Message(MessageDef.frame_input_c2s)]
    public static void frame_input_c2s(Uid uid, frame_input_c2s msg) {
        LockStep.Instance.AddInput(msg.Frame, uid, msg.Input);
    }
}