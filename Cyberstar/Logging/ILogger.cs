using System.Runtime.CompilerServices;

namespace Cyberstar.Logging;

public interface ILogger
{
    void Log(ELogLevel logLevel, string category, string message, Exception? e = null);
}

public static class LoggerExtensions
{
    public static void Debug(this ILogger logger, string message, [CallerMemberName] string? category = null, Exception? e = null)
    {
        logger.Log(ELogLevel.Debug, category ?? string.Empty, message, e);
    }
    
    public static void Warning(this ILogger logger, string message, [CallerMemberName] string? category = null, Exception? e = null)
    {
        logger.Log(ELogLevel.Warning, category ?? string.Empty, message, e);
    }
    
    public static void Error(this ILogger logger, string message, [CallerMemberName] string? category = null, Exception? e = null)
    {
        logger.Log(ELogLevel.Error, category ?? string.Empty, message, e);
    }
    
    public static void Critical(this ILogger logger, string message, [CallerMemberName] string? category = null, Exception? e = null)
    {
        logger.Log(ELogLevel.Critical, category ?? string.Empty, message, e);
    }
}