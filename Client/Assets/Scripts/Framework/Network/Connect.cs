using System;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

namespace Framework.Network {
    public class Connect {
        private string ip;
        private int port;
        
        private Socket socket;

        private Action<bool> callback;
        
        private enum ConnectState {
            Disconnected,
            Connecting,
            Connected,
        }
        
        private ConnectState state = ConnectState.Disconnected;

        private float? beginConnectTime = null;
        
        public void SetConfig(string ip, int port, Action<bool> callback) {
            this.ip = ip;
            this.port = port;
            this.callback = callback;
        }
        
        public bool IsSameConfig(string ip, int port) {
            return this.ip == ip && this.port == port;
        }

        private void InitSocket() {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 1000;
            socket.NoDelay = true;
        }

        public void BeginConnect() {
            DisConnect();
            
            state = ConnectState.Connecting;
            beginConnectTime = null;
            Updater.Instance.RegisterUpdate(OnUpdate);
            
            try {
                InitSocket();
                socket.BeginConnect(ip, port, ConnectComp, null);
            } catch (Exception e) {
                Log.Error("BeginConnect error: {0}", e.Message);
            }
        }

        private void ConnectComp(IAsyncResult ar) {
            try {
                socket.EndConnect(ar);
                state = ConnectState.Connected;
                Log.Info("Connected to {0}:{1}", ip, port);
                callback?.Invoke(true);
            } catch (Exception e) {
                state = ConnectState.Disconnected;
                Log.Error("ConnectComp error: {0}, ip: {1}, port: {2}", e.Message, ip, port);
                callback?.Invoke(false);
            }
        }
        
        public void DisConnect() {
            state = ConnectState.Disconnected;
            
            if (socket == null) {
                return;
            }
            try {
                if (socket.Connected) {
                    socket.Shutdown(SocketShutdown.Both);
                }
                socket.Close();
            } catch (Exception e) {
                Log.Error("DisConnect error: {0}", e.Message);
            }
            socket = null;
        }

        private void OnUpdate() {
            if (state != ConnectState.Connecting) {
                Updater.Instance.RemoveUpdate(OnUpdate);
                return;
            }

            if (beginConnectTime == null) {
                beginConnectTime = Time.realtimeSinceStartup;
            }

            if (Time.realtimeSinceStartup - beginConnectTime > ConnectConfig.ConnectTimeout) {
                DisConnect();
                callback?.Invoke(false);
                Log.Error("客户端连接超时 {0} s", ConnectConfig.ConnectTimeout);
            }
        }
    }
}