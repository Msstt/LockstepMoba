using System;

namespace Framework.Network {
    public class Network : Singleton<Network> {
        private NetworkState state = NetworkState.None;
        
        private Connect connect = new Connect();
        
        public void Connect(string ip, int port, bool isForce = false) {
            if (connect.IsSameConfig(ip, port) && !isForce) {
                if (state == NetworkState.Connecting || state == NetworkState.Connected) {
                    return;
                }
            }
            ChangeState(NetworkState.Connecting);
            connect.SetConfig(ip, port);
            connect.BeginConnect(ConnectComp);
        }

        private void ConnectComp(bool IsSuccess) {
            if (IsSuccess) {
                ChangeState(NetworkState.Connected);
            } else {
                ChangeState(NetworkState.Disconnected);
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