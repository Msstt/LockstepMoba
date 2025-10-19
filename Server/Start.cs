using Network;

void Main() {
    Console.WriteLine("当前目录: " + Directory.GetCurrentDirectory());
    if (!Config.Instance.ParseConfig()) {
        Console.WriteLine("Config parse failed!");
        return;
    }
    Console.WriteLine("Server is starting...");
    
    NetworkUtils.Start();
    
    while (true) {
        NetworkUtils.Update();
    }
}

Main();
