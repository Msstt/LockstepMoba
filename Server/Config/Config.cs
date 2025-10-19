using Framework;
using Newtonsoft.Json;

public class Config : Singleton<Config> {
    private static readonly string ConfigFilePath = Directory.GetCurrentDirectory() + "/../../../Config/Config/";

    public class NetworkConfig {
        public int port;
    };
    public NetworkConfig Network { get; private set; }

    public bool ParseConfig() {
        if (!ParseNetworkConfig()) return false;
        return true;
    }
    
    private bool ParseNetworkConfig() {
        Network = LoadFromFile<NetworkConfig>(ConfigFilePath + "network.json");
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