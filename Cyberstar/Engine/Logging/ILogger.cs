using System.Runtime.CompilerServices;

namespace Cyberstar.Engine.Logging;

public interface ILogger
{
    void Log(ELogLevel logLevel, string category, string message, Exception? e = null);
}

public static class LoggerExtensions
{
    public static void Debug(this ILogger logger, 
        string message, 
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, 
        [CallerLineNumber] int? lineNumber = null,
        Exception? e = null) => FormatLog(logger, ELogLevel.Debug, filePath ?? string.Empty, memberName ?? string.Empty, lineNumber ?? 0, message, e);
    
    public static void Warning(this ILogger logger, 
        string message, 
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, 
        [CallerLineNumber] int? lineNumber = null,
        Exception? e = null) => FormatLog(logger, ELogLevel.Warning, filePath ?? string.Empty, memberName ?? string.Empty, lineNumber ?? 0, message, e);
    
    public static void Error(this ILogger logger, 
        string message, 
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, 
        [CallerLineNumber] int? lineNumber = null,
        Exception? e = null) => FormatLog(logger, ELogLevel.Error, filePath ?? string.Empty, memberName ?? string.Empty, lineNumber ?? 0, message, e);
    
    public static void Critical(this ILogger logger, 
        string message, 
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, 
        [CallerLineNumber] int? lineNumber = null,
        Exception? e = null) => FormatLog(logger, ELogLevel.Critical, filePath ?? string.Empty, memberName ?? string.Empty, lineNumber ?? 0, message, e);

    private static void FormatLog(ILogger logger, ELogLevel logLevel, string filePath, string memberName, int lineNumber, string message, Exception? e)
    {
        var category = $"{filePath}#{memberName}-{lineNumber}";
        logger.Log(logLevel, category, message, e);
    }
}