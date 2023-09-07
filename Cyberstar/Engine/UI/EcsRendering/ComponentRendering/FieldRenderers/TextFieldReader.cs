namespace Cyberstar.Engine.UI.EcsRendering.ComponentRendering.FieldRenderers;

public readonly struct TextFieldReader<T> : FieldReader<T>
{
    private readonly Func<T, string> _getter;
        
    public TextFieldReader(string fieldName)
    {
        _getter = FieldReader<T>.CreateGetter<string>(typeof(T).GetField(fieldName)!);
    }

    public int GetStringValue(ref T source, Span<char> dest)
    {
        var fieldValue = _getter(source);
        var len = Math.Max(fieldValue.Length, dest.Length - 1);
        if (fieldValue.Length > len)
            fieldValue.AsSpan()
                .Slice(0, len)
                .CopyTo(dest);
        else
            fieldValue.CopyTo(dest);

        dest[len] = '\0';

        return len;
    }
}
