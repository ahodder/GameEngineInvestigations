namespace Cyberstar.Logging;

public class FileLogger : ILogger, IDisposable
{
    private StreamWriter _writer;
    
    
    public FileLogger(string filePath)
    {
        _writer = new StreamWriter(new FileStream(filePath, FileMode.Append));
    }
    
    public void Log(ELogLevel logLevel, string category, string message, Exception? e = null)
    {
        var msg = $"{DateTime.Now}: {logLevel} [{category}]- {message}";
        _writer.WriteLine(msg);
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}