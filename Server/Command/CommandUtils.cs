public static class CommandUtils {
    public static void HandleCommand() {
        string input = Console.ReadLine();
        int index = input.IndexOf(' ');
        string command = input, param = "";

        if (index >= 0) {
            command = input.Substring(0, index);
            param = input.Substring(index + 1);
        }
        
        if (CommandDef.Command.ContainsKey(command)) {
            CommandDef.Command[command](param);
        } else {
            Console.WriteLine($"Unknown command: {command}");
        }
    }
}