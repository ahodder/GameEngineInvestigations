using Cyberstar.AssetManagement;
using Raylib_cs;

namespace Cyberstar.UI.ViewFragments;

public struct BackgroundFragment : IViewFragment
{
    public Color BackgroundColor;

    public void Render(AssetManager assetManager, in ViewDimensions viewDimensions)
    {
        var x = viewDimensions.PaddedStartX;
        var y = viewDimensions.PaddedStartY;
        var w = viewDimensions.PaddedWidth;
        var h = viewDimensions.PaddedHeight;
        
        Raylib.DrawRectangle(x, y, w, h, BackgroundColor);
    }
}