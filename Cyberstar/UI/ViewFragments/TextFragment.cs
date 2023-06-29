using Raylib_cs;

namespace Cyberstar.UI.ViewFragments;

public struct TextFragment
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
}

public enum ETextAlignment
{
    Left,
    Center,
    Right,
}