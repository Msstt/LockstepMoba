using Framework;
using Newtonsoft.Json;

public class Config : Singleton<Config> {
    private string ConfigFilePath { get; set; } = Path.GetFullPath(
        Path.Combine(Directory.GetCurrentDirectory(), "../../../Config/Config"));
    
    public NetworkConfig Network { get; private set; }

    public void OverrideConfigFilePath(string path) {
        ConfigFilePath = Path.GetFullPath(path);
        Console.WriteLine($"[Config] config path: {ConfigFilePath}");
    }

    public bool ParseConfig() {
        if (!ParseNetworkConfig()) return false;
        return true;
    }
    
    private bool ParseNetworkConfig() {
        Network = LoadFromFile<NetworkConfig>(Path.Combine(ConfigFilePath, "network.json"));
        if (Network == null) {
            return false;
        }
        return true;
    }
    
    private static T LoadFromFile<T>(string path) {
        try {
            if (!File.Exists(path)) {
                Console.WriteLine($"[Config] {path} not exists");
                return default;
            }
                
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        } catch (Exception e) {
            Console.WriteLine($"[Config] parse failed, {e}");
            return default;
        }
    }
}
