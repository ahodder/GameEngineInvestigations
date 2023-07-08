using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Cyberstar.AssetManagement;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Cyberstar.UI;

public class NoMemLabelView : ViewBase
{
    public delegate ReadOnlySpan<char> TextProvider();
    
    public Font Font { get; set; }
    public TextProvider Text { get; set; }
    public float FontSize { get; set; }
    public float Spacing { get; set; }
    public Color TextColor { get; set; }

    public NoMemLabelView(AssetManager assetManager) : base(assetManager)
    {
        Font = assetManager.FontAtlas.DefaultFont;
        FontSize = 12f;
        Spacing = 1f;
        TextColor = Color.BLACK;
        BackgroundColor = Color.WHITE;
    }
    
    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        var textSpan = Text.Invoke();
        Vector2 vec;
        unsafe
        {
            sbyte* bytes = stackalloc sbyte[textSpan.Length];
            for (var i = 0; i < textSpan.Length; i++)
            {
                bytes[i] = (sbyte)textSpan[i];
            }
            vec = Raylib.MeasureTextEx(Font, bytes, FontSize, Spacing); 
        }
        return new Point((int)vec.X, (int)vec.Y);
    }

    protected override void DoRenderContent(in InputData inputData)
    {
        var textSpan = Text.Invoke();
        unsafe
        {
            sbyte* bytes = stackalloc sbyte[textSpan.Length];
            for (var i = 0; i < textSpan.Length; i++)
            {
                bytes[i] = (sbyte)textSpan[i];
            }
            Raylib.DrawTextEx(Font, bytes, new Vector2(ContentBounds.X, ContentBounds.Y), FontSize, Spacing, TextColor);
        }
    }
}