using Network;

void Main() {
    Console.WriteLine("Server is starting...");
    
    Framework.Network.Network.Instance.Start(9980);
    
    Framework.Network.Network.Instance.RegisterMsgHandler(MessageDef.Test, (msg) => {
        Framework.Network.Network.Instance.Send(msg);
    });

    while (true) {
        Framework.Network.Network.Instance.DispatchMsg();
    }
}

Main();
