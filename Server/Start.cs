using Network;

void Main() {
    Console.WriteLine("Server is starting...");
    
    NetworkUtils.Start();
    
    while (true) {
        NetworkUtils.Update();
    }
}

Main();
