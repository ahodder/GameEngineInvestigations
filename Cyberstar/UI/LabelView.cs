using System.Drawing;
using System.Numerics;
using Cyberstar.AssetManagement;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Cyberstar.UI;

public class LabelView : ViewBase
{
    public Font Font { get; set; }

    public ReadOnlySpan<char> Text
    {
        get => _charBuffer.AsSpan(_charCount);
        set
        {
            value.CopyTo(_charBuffer);
            _charCount = value.Length;
            _charBuffer[value.Length] = (char)0;
        }
    }
    public float FontSize { get; set; }
    public float Spacing { get; set; }
    public Color TextColor { get; set; }

    private readonly char[] _charBuffer = new char[512];
    private int _charCount;

    public LabelView(AssetManager assetManager) : base(assetManager)
    {
        Font = assetManager.FontAtlas.DefaultFont;
        FontSize = 12f;
        Spacing = 1f;
        TextColor = Color.BLACK;
        BackgroundColor = Color.WHITE;

        _charCount = 0;
    }

    public void SetText(ReadOnlySpan<char> text)
    {
        text.CopyTo(_charBuffer);
        _charCount = text.Length;
    }
    
    protected override unsafe Point DoMeasure(int x, int y, int width, int height)
    {
        var bytes = stackalloc sbyte[_charCount];
        for (var i = 0; i < _charCount; i++)
            bytes[i] = (sbyte)_charBuffer[i];
        var vec = Raylib.MeasureTextEx(Font, bytes, FontSize, Spacing);
        
        return new Point((int)vec.X, (int)vec.Y);
    }

    protected override unsafe void DoRenderContent(in InputData inputData)
    {
        var bytes = stackalloc sbyte[_charCount];
        for (var i = 0; i < _charCount; i++)
            bytes[i] = (sbyte)_charBuffer[i];
        Raylib.DrawTextEx(Font, bytes, new Vector2(ContentBounds.X, ContentBounds.Y), FontSize, Spacing, TextColor);
    }
}