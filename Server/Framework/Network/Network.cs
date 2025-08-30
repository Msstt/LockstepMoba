

namespace Framework.Network {
    public class Network {
        int port;
        
        public Network(int port) {
            this.port = port;
        }

        public void Start() {
            Listener listener = new Listener();
            if (!listener.Listen(port)) {
                return;
            }
            while (true) {
                var client = listener.Accept();
            }
        }
    }
}
