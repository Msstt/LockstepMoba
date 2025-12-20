using System;
using Framework.Network;
using Google.Protobuf;
using Network;

public class MsgDispatcher {
    protected static void Register<T>(MessageDef msgId, Action<T> handler) where T : IMessage {
        GameMgr.Instance.GetSystem<INetwork>().RegisterHandler(msgId, handler);
    }
}
