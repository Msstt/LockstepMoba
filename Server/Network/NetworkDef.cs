namespace Network {
    public static class NetworkDef {
        public static void RegisterDispatcher() {
            TestMsgDispatcher.Register();
            FrameMsgDispatcher.Register();
        }
    }
}
