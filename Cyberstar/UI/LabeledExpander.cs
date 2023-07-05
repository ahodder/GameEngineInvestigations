using Cyberstar.AssetManagement;
using Cyberstar.UI.ViewFragments;

namespace Cyberstar.UI;

public struct LabeledExpander<T> : IView where T : IView
{
    public ViewDimensions Dimensions { get; }

    public Label Label;
    public T ExpanderView;
    public bool IsExpanded;

    public void Render(AssetManager assetManager)
    {
    }
}