namespace Framework.Network {
    public enum NetworkState {
        None,
        Connecting,
        Connected,
        Disconnected,
        Reconnecting,
        NoNetwork,
    }
    
    public static class ConnectConfig {
        public static int MaxReconnectCount = 5;
        public static float ConnectTimeout = 10;
    }

    public static class SendConfig {
        public static int MaxQueueCount = 1024;
    }
    
    public static class ReceiveConfig {
        public static int BufferSize = 1024 * 1024;
        public static int DispatchCountPerFrame = 10;
    }

    public static class NetworkLogConfig {
        public static bool EnableMessageLog = true;
    }
}