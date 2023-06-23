namespace Cyberstar.Core;

public readonly struct Result<T>
{
    public static readonly Result<T> DefaultSuccess = new Result<T>(default(T));

    public readonly T Data;
    public readonly bool Success;
    public readonly string? ErrorMessage;
    public readonly Exception? Exception;

    public Result(T data)
    {
        Data = data;
        Success = true;
        ErrorMessage = null;
        Exception = null;
    }

    public Result(string errorMessage, Exception? exception = null)
    {
        Data = default(T);
        Success = false;
        ErrorMessage = errorMessage;
        Exception = exception;
    }
}

public static class ResultExtensions
{
    public static Result<T> Success<T>(this T self) => new Result<T>(self);

    public static Result<T> Error<T>(this object self, string errorMessage, Exception? error = null) => new (errorMessage, error);
    public static Result<T> Error<T>(this string errorMessage, Exception? error = null) => new (errorMessage, error);
    public static Result<TOther> Wrap<T, TOther>(this Result<T> target) => new (target.ErrorMessage, target.Exception);
}

