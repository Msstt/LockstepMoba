using System.Net.Sockets;

namespace Framework.Network {
    public class Listener {
        private TcpListener socket;

        public bool Listen(int port) {
            socket = new TcpListener(System.Net.IPAddress.Any, port);
            try {
                socket.Start();
            } catch (SocketException e) {
                Log.Error("Failed to listen on port {0}: {1}", port, e.Message);
                return false;
            }
            Log.Info("Listening on port {0}", port);
            return true;
        }

        public TcpClient? Accept() {
            TcpClient? ret = null;
            try {
                ret = socket.AcceptTcpClient();
            } catch (SocketException e) {
                ret = null;
                Log.Error("Failed to accept tcp client: {0}", e.Message);
            }
            if (ret != null) {
                Log.Info("Accepted tcp client: {0}", ret.Client.RemoteEndPoint);
            }
            return ret;
        }
    }
}
