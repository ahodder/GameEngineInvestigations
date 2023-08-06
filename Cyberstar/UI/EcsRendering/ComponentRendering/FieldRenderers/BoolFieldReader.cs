namespace Cyberstar.UI.EcsRendering.ComponentRendering.FieldRenderers;

public readonly struct BoolFieldReader<T> : FieldReader<T>
{
    private readonly Func<T, bool> _getter;
        
    public BoolFieldReader(string fieldName)
    {
        _getter = FieldReader<T>.CreateGetter<bool>(typeof(T).GetField(fieldName)!);
    }

    public int GetStringValue(ref T source, Span<char> dest)
    {
        var fieldValue = _getter(source);
        if (fieldValue.TryFormat(dest, out var charsWritten))
        {
            dest[charsWritten] = '\0';
            return charsWritten + 1;
        }

        dest[0] = '\0';
        return 1;
    }
}
