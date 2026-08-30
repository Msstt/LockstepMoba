using Network;

void Main(string[] args) {
    for (int i = 0; i < args.Length; i++) {
        if (args[i] != "--config") continue;

        if (i + 1 >= args.Length) {
            Console.WriteLine("Missing value for --config");
            return;
        }

        Config.Instance.OverrideConfigFilePath(args[++i]);
    }

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
        AsyncUtils.Update();
        Thread.Sleep(1);
    }
}

Main(args);
