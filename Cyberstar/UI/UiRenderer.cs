using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.UI.ViewFragments;
using Raylib_cs;

namespace Cyberstar.UI;

public class UiRenderer
{
    private readonly FontAtlas _fontAtlas;
    
    public UiRenderer(FontAtlas fontAtlas)
    {
        _fontAtlas = fontAtlas;
    }

    /// <summary>
    /// Draws a button to the screen.
    /// </summary>
    /// <param name="button"></param>
    public void DrawButton(in Button button)
    {
        RenderBackgroundFragment(button.Dimensions, button.Background);
        RenderTextFragment(button.Dimensions, button.Text);

        var mousePosition = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && button.Dimensions.Contains(mousePosition))
        {
            button.OnClick?.Invoke();
        }
    }

    /// <summary>
    /// Draws a label to the screen.
    /// </summary>
    /// <param name="label"></param>
    public void DrawLabel(in Label label)
    {
        RenderTextFragment(label.Dimensions, label.Text);
    }

    /// <summary>
    /// Renders some text to the given dimensions.
    /// </summary>
    /// <param name="dimensionsFragment"></param>
    /// <param name="textFragment"></param>
    private void RenderTextFragment(in DimensionsFragment dimensionsFragment, in TextFragment textFragment)
    {
        var pos = new Vector2(dimensionsFragment.ContentStartX, dimensionsFragment.ContentStartY);

        Raylib.DrawTextEx(textFragment.Font, textFragment.Text, pos, textFragment.FontSize, 1, textFragment.TextColor);
    }

    /// <summary>
    /// Renders some background fragment.
    /// </summary>
    /// <param name="dimensionsFragment"></param>
    /// <param name="backgroundFragment"></param>
    private void RenderBackgroundFragment(in DimensionsFragment dimensionsFragment, in ViewBackgroundFragment backgroundFragment)
    {
        var x = dimensionsFragment.PaddedStartX;
        var y = dimensionsFragment.PaddedStartY;
        var w = dimensionsFragment.PaddedWidth;
        var h = dimensionsFragment.PaddedHeight;
        
        Raylib.DrawRectangle(x, y, w, h, backgroundFragment.BackgroundColor);
        
        Raylib.DrawRectangleLines(dimensionsFragment.ActualX, 
            dimensionsFragment.ActualY,
            dimensionsFragment.ActualWidth,
            dimensionsFragment.ActualHeight,
            backgroundFragment.BackgroundColor);
    }
}