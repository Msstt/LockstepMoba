using System;

namespace Framework.Network {
    public class Network : Singleton<Network> {
        private NetworkState state = NetworkState.None;
        
        private Connect connect = new Connect();
        private Writer writer = new Writer();
        
        private int reconnectCount = 0;
        
        private bool needDisconnect = false;

        public Network() {
            Updater.Instance.RegisterUpdate(CheckDisconnect);
        }

        #region 连接
        
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
            
            ThreadMgr.Instance.Start(ThreadTaskId.SocketWrite);
        }

        private void ConnectComp(bool isSuccess) {
            if (isSuccess) {
                ChangeState(NetworkState.Connected);
                writer.SetSocket(connect.Socket);
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
        
        public void Disconnect() {
            connect.Disconnect();
            ChangeState(NetworkState.Disconnected);
            Log.Info("客户端断开连接");
        }
        
        public void TryDisconnect() {
            needDisconnect = true;
        }

        public void CheckDisconnect() {
            if (needDisconnect) {
                Disconnect();
                needDisconnect = false;
            }
        }

        #endregion

        #region 发送

        public void Send(Message msg) {
            writer.Send(msg);
            
            Log.Info("<color=green>Send To " + msg.ToString() + "</color>");
        }
        
        public void FlushWrite() {
            writer.Flush();
        }

        #endregion


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