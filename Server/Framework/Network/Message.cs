using Network;
using Google.Protobuf;

namespace Framework.Network {
    public struct Message {
        public Client client;
        public MessageDef msgId;
        public IMessage data;

        public override string ToString() {
            return string.Format("ID: {0}, Data: {1}", msgId, data);
        }
    }
}