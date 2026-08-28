using Framework;
using Network;

public class FileLog : Singleton<FileLog> {
    private static readonly string FolderPath = Directory.GetCurrentDirectory() + "/../../../Log/";
    private string filePath;
    
    public FileLog() {
        if (!Directory.Exists(FolderPath)) {
            Directory.CreateDirectory(FolderPath);
        }
        
        filePath = FolderPath + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
        using (File.Create(filePath)) { }
    }

    public void Log(string message) {
        lock (this) {
            File.AppendAllText(filePath, message + "\n\n");
        }
    }

    public string RecordInputData(battle_start_s2c startMsg, List<Dictionary<Uid, battle_input>> historyInputs) {
        string filePath = FolderPath + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_input.txt";
        string record = startMsg + "\n";
        for (int i = 0; i < historyInputs.Count; i++) {
            record += $"Frame {i + 1}:\n";
            foreach (var (uid, input) in historyInputs[i]) {
                record += $"  Uid: {uid}, Input: {input}\n";
            }
        }
        File.WriteAllText(filePath, record);
        return filePath;
    }
}