using System.Numerics;
using Cyberstar.AssetManagement;
using Raylib_cs;

namespace Cyberstar.UI.ViewFragments;

public struct TextFragment : IViewFragment
{
    public Font Font;
    public string Text;
    public int FontSize;
    public Color TextColor;
    public ETextAlignment HorizontalTextAlignment;
    public ETextAlignment VerticalTextAlignment;

    public TextFragment(Font font, string text)
    {
        Text = text;
        Font = font;
    }

    public void Render(AssetManager assetManager, in ViewDimensions viewDimensions)
    {
        var pos = new Vector2(viewDimensions.ContentStartX, viewDimensions.ContentStartY);

        Raylib.DrawTextEx(Font, Text, pos, FontSize, 1, TextColor);
    }
}

public enum ETextAlignment
{
    Left,
    Center,
    Right,
}