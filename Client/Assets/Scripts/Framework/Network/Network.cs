using System;

namespace Framework.Network {
    public class Network : Singleton<Network> {
        private NetworkState state = NetworkState.None;
        
        private Connect connect = new Connect();
        
        private int reconnectCount = 0;
        
        public void Connect(string ip, int port, bool isForce = false) {
            if (connect.IsSameConfig(ip, port) && !isForce) {
                if (state == NetworkState.Connecting || state == NetworkState.Connected) {
                    return;
                }
            }
            reconnectCount = 0;
            ChangeState(NetworkState.Connecting);
            connect.SetConfig(ip, port, ConnectComp);
            connect.BeginConnect();
        }

        private void ConnectComp(bool isSuccess) {
            if (isSuccess) {
                ChangeState(NetworkState.Connected);
            } else {
                if (reconnectCount < ConnectConfig.MaxReconnectCount) {
                    reconnectCount += 1;
                    ChangeState(NetworkState.Reconnecting);
                    connect.BeginConnect();
                } else {
                    ChangeState(NetworkState.Disconnected);
                }
            }
        }

        #region 事件

        private Action<NetworkState> stateChangeHandlers = null;

        public void RegisterStateChangeEvent(Action<NetworkState> handler) {
            stateChangeHandlers += handler;
        }
        
        public void RemoveStateChangeEvent(Action<NetworkState> handler) {
            stateChangeHandlers -= handler;
        }

        public void ChangeState(NetworkState state) {
            this.state = state;
            stateChangeHandlers?.Invoke(state);
        }

        #endregion
    }
}