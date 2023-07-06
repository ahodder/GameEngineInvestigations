using System.Drawing;
using System.Numerics;
using Cyberstar.AssetManagement;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Cyberstar.UI;

public class LabelView : ViewBase
{
    public Font Font { get; set; }
    public string Text { get; set; }
    public float FontSize { get; set; }
    public float Spacing { get; set; }
    public Color TextColor { get; set; }

    public LabelView(AssetManager assetManager) : base(assetManager)
    {
        Font = assetManager.FontAtlas.DefaultFont;
        Text = string.Empty;
        FontSize = 12f;
        Spacing = 1f;
        TextColor = Color.BLACK;
        BackgroundColor = Color.WHITE;
    }
    
    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        var vec = Raylib.MeasureTextEx(Font, Text, FontSize, Spacing); 
        return new Point((int)vec.X, (int)vec.Y);
    }

    protected override void DoRenderContent()
    {
        Raylib.DrawTextEx(Font, Text, new Vector2(ContentBounds.X, ContentBounds.Y), FontSize, Spacing, TextColor);
    }
}