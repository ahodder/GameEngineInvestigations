using Cyberstar.AssetManagement;

namespace Cyberstar.UI;

public struct LinearLayoutView : IView
{
    public ViewDimensions Dimensions { get; }

    /* todo ahodder@quantum-intl.com 7/4/23: Remove this allocation? */
    public List<IView> Children { get; } = new List<IView>();

    public LinearLayoutView()
    {
    }


    public void Render(AssetManager assetManager)
    {
        throw new NotImplementedException();
    }
}