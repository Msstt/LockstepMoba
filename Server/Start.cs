using Network;

void Main() {
    if (!Config.Instance.ParseConfig()) {
        Console.WriteLine("Config parse failed!");
        return;
    }
    Console.WriteLine("Server is starting...");
    
    NetworkUtils.Start();
    
    Task.Run(() => {
        while (true) {
            CommandUtils.HandleCommand();
            Thread.Sleep(1);
        }
    });
    
    while (true) {
        NetworkUtils.Update();
        Thread.Sleep(1);
    }
}

Main();
