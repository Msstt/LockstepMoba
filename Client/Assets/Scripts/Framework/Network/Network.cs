using System;
using Google.Protobuf;
using Network;
using UnityEngine;

namespace Framework.Network {
    public class Network : INetwork {
        private NetworkState state = NetworkState.None;
        
        private Connect connect = new Connect();
        private Writer writer = null;
        private Reader reader = null;
        
        private int reconnectCount = 0;
        
        private bool needDisconnect = false;
        
        private MsgDispatcher msgDispatcher = new MsgDispatcher();

        public Network() {
            writer = new Writer(this);
            reader = new Reader(this);
            
            UnityEventMgr.Instance.Register(UnityEventType.OnUpdate, CheckDisconnect);
            UnityEventMgr.Instance.Register(UnityEventType.OnQuit, () => {
                Disconnect();
            });
        }

        public void Start() {
            NetworkDef.RegisterDispatcher();
            
            Connect("127.0.0.1", 9980);
            
            Send(MessageDef.frame_reconnect_c2s, new frame_reconnect_c2s {
                Frame = 1,
            });
        }

        public void Update() {
            DispatchMsg();
        }

        public void FrameStart() { }
        public void FrameUpdate() { }

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
            
            ThreadMgr.Instance.Start(ThreadTaskId.SocketWrite, FlushWrite);
            ThreadMgr.Instance.Start(ThreadTaskId.SocketRead, FlushRead);
        }

        private void ConnectComp(bool isSuccess) {
            if (isSuccess) {
                ChangeState(NetworkState.Connected);
                writer.SetSocket(connect.Socket);
                reader.SetSocket(connect.Socket);
                
                EventMgr.Instance.Send(new EventType.OnConnected());
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
            Log.Error("客户端断开连接");
        }
        
        public void TryDisconnect() {
            needDisconnect = true;
        }

        private void CheckDisconnect() {
            if (needDisconnect) {
                Disconnect();
                needDisconnect = false;
            }
        }

        #endregion

        #region 发送

        public void FlushWrite() {
            writer.Flush();
        }
        
        public void Send(Message msg) {
            if (state != NetworkState.Connected) {
                return;
            }
            
            writer.Send(msg);
            Debug.Log("<color=green>Send To " + msg + "</color>");
        }
        
        public void Send(MessageDef msgId, IMessage data) {
            if (state != NetworkState.Connected || data == null) {
                return;
            }

            Message msg = new Message() {
                msgId = msgId,
                data = data,
            };
            
            writer.Send(msg);
            Debug.Log("<color=green>Send To " + msg + "</color>");
        }
        
        #endregion

        #region 接收

        public void FlushRead() {
            reader.Flush();
        }
        
        public void DispatchMsg() {
            for (int i = 0; i < ReceiveConfig.DispatchCountPerFrame; i++) {
                Message? msg = reader.GetMessage();
                if (msg == null) {
                    return;
                }
                Debug.Log("<color=orange>Receive From " + msg + "</color>");
                msgDispatcher.Dispatch(msg.Value);
            }
        }
        
        public void RegisterHandler<T>(MessageDef msgId, Action<T> handler) where T : IMessage {
            msgDispatcher.RegisterHandler<T>(msgId, handler);
        }

        public void UnRegisterHandler<T>(MessageDef msgId, Action<T> handler) where T : IMessage {
            msgDispatcher.UnRegisterHandler<T>(msgId, handler);
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