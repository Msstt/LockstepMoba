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
        public static int ConnectTimeout = 10;
    }
}