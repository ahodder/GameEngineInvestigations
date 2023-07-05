using Cyberstar.AssetManagement;

namespace Cyberstar.UI.ViewFragments;

public interface IViewFragment
{
    void Render(AssetManager assetManager, in ViewDimensions dimensions);
}