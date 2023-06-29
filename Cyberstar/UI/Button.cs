using Cyberstar.UI.ViewFragments;
using Raylib_cs;

namespace Cyberstar.UI;

public struct Button
{
    public DimensionsFragment Dimensions;
    public ViewBackgroundFragment Background;
    public TextFragment Text;
    public Action? OnClick;

    public Button(
        Font font,
        string buttonText,
        int x,
        int y,
        int fontSize = 12)
    {
        var size = Raylib.MeasureTextEx(font, buttonText, fontSize, 1);
        
        Text = new TextFragment
        {
            Font = font,
            Text = buttonText,
            FontSize = fontSize,
            TextColor = Color.WHITE,
        };

        Dimensions = new DimensionsFragment(x, y, (int)MathF.Ceiling(size.X), (int)MathF.Ceiling(size.Y));
    }

    public Button(
        Font font,
        string buttonText, 
        int x, 
        int y, 
        int width, 
        int height,
        int fontSize = 12)
    {
        Text = new TextFragment
        {
            Font = font,
            Text = buttonText,
            FontSize = fontSize,
            TextColor = Color.WHITE,
        };

        Dimensions = new DimensionsFragment(x, y, width, height);
    }
}