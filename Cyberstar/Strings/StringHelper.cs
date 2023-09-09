using System.Text;

namespace Cyberstar.Strings;

public static class StringHelper
{
    public static bool Compare(ReadOnlySpan<char> source, Span<sbyte> target)
    {
        /* todo ahodder@praethos.com 9/9/23: This needs to be simdatized. Create one vector, sub == 0 true */
        if (source.Length != target.Length) return false;
        for (var i = 0; i < source.Length; i++)
            if (source[i] != target[i])
                return false;

        return true;
    }

    public static unsafe bool Compare(ReadOnlySpan<char> source, sbyte* target, int targetLength)
    {
        /* todo ahodder@praethos.com 9/9/23: This needs to be simdatized. Create one vector, sub == 0 true */
        if (source.Length != targetLength) return false;
        for (var i = 0; i < source.Length; i++)
            if (source[i] != target[i])
                return false;

        return true;
    }

    public static unsafe int FillFromCString(this Span<char> dest, sbyte* cstr)
    {
        var i = 0;
        while (cstr[i] != 0)
        {
            dest[i] = (char)cstr[i++];
        }

        dest[i] = '\0';
        return i;
    }
}