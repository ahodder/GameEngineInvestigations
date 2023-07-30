namespace Cyberstar.Engine.Logging;

public class ConsoleLogger : ILogger
{
    public void Log(ELogLevel logLevel, string category, string message, Exception? e = null)
    {
        Console.WriteLine($"[{logLevel}] {DateTime.Now} {category}\n{message} {(e != null ? e : string.Empty)}");
    }
}