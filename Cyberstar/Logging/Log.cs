using System.Runtime.CompilerServices;

namespace Cyberstar.Logging;

public static class Log
{
    public static ILogger Instance { get; set; } = new ConsoleLogger();

    public static void Information(
        string message,
        Exception? e = null, 
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, 
        [CallerLineNumber] int? lineNumber = null) => FormatLog(Instance, ELogLevel.Information, filePath ?? string.Empty, memberName ?? string.Empty, lineNumber ?? 0, message, e);
    
    public static void Debug(
        string message,
        Exception? e = null, 
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, 
        [CallerLineNumber] int? lineNumber = null) => FormatLog(Instance, ELogLevel.Debug, filePath ?? string.Empty, memberName ?? string.Empty, lineNumber ?? 0, message, e);
    
    public static void Warning(
        string message,
        Exception? e = null, 
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, 
        [CallerLineNumber] int? lineNumber = null) => FormatLog(Instance, ELogLevel.Warning, filePath ?? string.Empty, memberName ?? string.Empty, lineNumber ?? 0, message, e);
    
    public static void Error(
        string message,
        Exception? e = null, 
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, 
        [CallerLineNumber] int? lineNumber = null) => FormatLog(Instance, ELogLevel.Error, filePath ?? string.Empty, memberName ?? string.Empty, lineNumber ?? 0, message, e);
    
    public static void Critical(
        string message,
        Exception? e = null, 
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, 
        [CallerLineNumber] int? lineNumber = null) => FormatLog(Instance, ELogLevel.Critical, filePath ?? string.Empty, memberName ?? string.Empty, lineNumber ?? 0, message, e);

    private static void FormatLog(ILogger logger, ELogLevel logLevel, string filePath, string memberName, int lineNumber, string message, Exception? e)
    {
        var category = $"{filePath}#{memberName}-{lineNumber}";
        logger.Log(logLevel, category, message, e);
    }
}