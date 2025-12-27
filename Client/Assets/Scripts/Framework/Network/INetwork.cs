using System;
using Google.Protobuf;
using Network;

namespace Framework.Network {
    public interface INetwork : IInitSystem, IUpdateSystem {
        public void Connect(string ip, int port, bool isForce = false);
        public void Disconnect();

        public void Send(MessageDef msgId, IMessage data);

        public void RegisterHandler<T>(MessageDef msgId, Action<T> handler) where T : IMessage;

        public void UnRegisterHandler<T>(MessageDef msgId, Action<T> handler) where T : IMessage;
    }
}