using System.Linq.Expressions;
using System.Reflection;

namespace Cyberstar.UI.EcsRendering.ComponentRendering.FieldRenderers;

public interface FieldReader<T>
{
    /// <summary>
    /// Gets the field value as a string.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns>The number of characters written into the span.</returns>
    int GetStringValue(ref T source, Span<char> dest);
        
    public static Func<T, TResult> CreateGetter<TResult>(FieldInfo fieldInfo)
    {
        var parameter = Expression.Parameter(typeof(T));
        var field = Expression.Field(parameter, fieldInfo);
        var getter = Expression.Lambda<Func<T, TResult>>(field, parameter).Compile();
        return getter;
    } 
}
