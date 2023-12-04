namespace Codecool.MarsExploration.MapExplorer.Logger;

public class FileLogger : ILogger
{
    private static string _fileName;
    private static string _directoryPath;

    public FileLogger(string fileName, string directoryPath)
    {
        _fileName = fileName;
        _directoryPath = directoryPath;

    }
    public void Log(string message)
    {
        try
        {
            using var sw = new StreamWriter($"{_directoryPath}/{_fileName}", append: true);
            sw.WriteLine($"[{DateTime.Now}]: {message}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong...");
        }
        
    }

    public static void CreateLogFile(string directoryPath, string fileName)
    {
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        if (!File.Exists($"{directoryPath}/{fileName}")) File.Create($"{directoryPath}/{fileName}");
    }
}