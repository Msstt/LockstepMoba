
void Main() {
    Console.WriteLine("Server is starting...");
    
    Framework.Network.Network.Instance.Start(9980);
    
    while (true) {}
}

Main();
