using Framework;

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
}