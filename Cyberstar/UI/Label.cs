using Cyberstar.AssetManagement;
using Cyberstar.UI.ViewFragments;

namespace Cyberstar.UI;

public struct Label : IView
{
    public ViewDimensions Dimensions { get; }
    public TextFragment Text;

    public void Render(AssetManager assetManager)
    {
        Text.Render(assetManager, Dimensions);
    }
}