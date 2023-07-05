using Cyberstar.AssetManagement;
using Cyberstar.UI.ViewFragments;

namespace Cyberstar.UI;

public interface IView
{
    ViewDimensions Dimensions { get; }

    void Render(AssetManager assetManager);
}