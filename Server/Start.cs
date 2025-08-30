
void Main() {
    Console.WriteLine("Server is starting...");
    
    Framework.Network.Network network = new Framework.Network.Network(9980);
    network.Start();
}

Main();
