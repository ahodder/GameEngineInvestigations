using System.Drawing;
using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
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
    public float FontSpacing { get; set; }
    public Color TextColor { get; set; }

    private readonly char[] _charBuffer = new char[64];
    private int _charCount;

    public LabelView(AssetManager assetManager) : base(assetManager)
    {
        Font = assetManager.FontAtlas.DefaultFont;
        FontSize = 12f;
        FontSpacing = 1f;
        TextColor = Color.BLACK;
        BackgroundColor = Color.WHITE;
    }

    public LabelView(AssetManager assetManager, 
        ReadOnlySpan<char> text,
        Font? font = null,
        float fontSize = 18,
        float fontFontSpacing = 1f) : base(assetManager)
    {
        Text = text;
        Font = font ?? assetManager.FontAtlas.DefaultFont;
        FontSize = fontSize;
        FontSpacing = fontFontSpacing;
        TextColor = Color.WHITE;
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
        var vec = Raylib.MeasureTextEx(Font, bytes, FontSize, FontSpacing);
        
        return new Point((int)vec.X, (int)vec.Y);
    }

    protected override unsafe void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        var bytes = stackalloc sbyte[_charCount];
        for (var i = 0; i < _charCount; i++)
            bytes[i] = (sbyte)_charBuffer[i];
        Raylib.DrawTextEx(Font, bytes, new Vector2(ContentBounds.X, ContentBounds.Y), FontSize, FontSpacing, TextColor);
    }
}