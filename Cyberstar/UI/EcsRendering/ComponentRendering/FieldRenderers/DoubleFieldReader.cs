namespace Cyberstar.UI.EcsRendering.ComponentRendering.FieldRenderers;

public class DoubleFieldReader<T> : FieldReader<T>
{
    private readonly Func<T, double> _getter;

    public DoubleFieldReader(string fieldName)
    {
        _getter = FieldReader<T>.CreateGetter<double>(typeof(T).GetField(fieldName)!);
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