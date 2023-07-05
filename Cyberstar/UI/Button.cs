using Cyberstar.AssetManagement;
using Cyberstar.UI.ViewFragments;
using Raylib_cs;

namespace Cyberstar.UI;

public struct Button : IView
{
    public ViewDimensions Dimensions { get; }
    public BackgroundFragment Background;
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
            TextColor = Color.BLACK,
        };
        
        Dimensions = new ViewDimensions(x, y, (int)MathF.Ceiling(size.X), (int)MathF.Ceiling(size.Y));
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

        Dimensions = new ViewDimensions(x, y, width, height);
    }

    public void Render(AssetManager assetManager)
    {
        Background.Render(assetManager, Dimensions);
        Text.Render(assetManager, Dimensions);
        
        var mousePosition = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && Dimensions.Contains(mousePosition))
        {
            OnClick?.Invoke();
        }
    }
}