
public static class CommandDef {
    public static readonly Dictionary<string, Action<string>> Command = new Dictionary<string, Action<string>>() {
        { "start", (_) => {
            Console.WriteLine("Battle is starting...");
        } },
    };
}