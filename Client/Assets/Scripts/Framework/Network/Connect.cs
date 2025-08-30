using System;
using System.Net.Sockets;

namespace Framework.Network {
    public class Connect {
        private string ip;
        private int port;
        
        private Socket socket;

        private Action<bool> callback;
        
        public void SetConfig(string ip, int port) {
            this.ip = ip;
            this.port = port;
        }
        
        public bool IsSameConfig(string ip, int port) {
            return this.ip == ip && this.port == port;
        }

        private void InitSocket() {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 1000;
            socket.NoDelay = true;
        }

        public void BeginConnect(Action<bool> callback) {
            this.callback = callback;
            DisConnect();
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
                Log.Info("Connected to {0}:{1}", ip, port);
                callback?.Invoke(true);
            } catch (Exception e) {
                Log.Error("ConnectComp error: {0}, ip: {1}, port: {2}", e.Message, ip, port);
                callback?.Invoke(false);
            }
        }
        
        public void DisConnect() {
            if (socket == null) {
                return;
            }
            try {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            } catch (Exception e) {
                Log.Error("DisConnect error: {0}", e.Message);
            }
            socket = null;
        }
    }
}