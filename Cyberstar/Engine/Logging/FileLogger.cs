namespace Cyberstar.Engine.Logging;

public class FileLogger : ILogger, IDisposable
{
    private StreamWriter _writer;
    
    
    public FileLogger(string filePath)
    {
        _writer = new StreamWriter(new FileStream(filePath, FileMode.Append));
    }
    
    public void Log(ELogLevel logLevel, string category, string message, Exception? e = null)
    {
        lock (_writer)
        {
            var msg = $"[{logLevel}] {DateTime.Now} {category}\n{message}\n{(e != null ? e : string.Empty)}";
            _writer.WriteLine(msg);
        }
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}