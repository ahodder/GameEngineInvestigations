using System.Drawing;
using Cyberstar.AssetManagement;

namespace Cyberstar.UI;

public class LabeledExpanderView : ViewBase
{
    public LabelView LabelView { get; set; }
    public VerticalLinearLayoutView ExpanderView { get; set; }
    public bool IsExpanded { get; set; }

    public LabeledExpanderView(AssetManager assetManager) : base(assetManager)
    {
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        throw new NotImplementedException();
    }

    protected override void DoRenderContent(in InputData inputData)
    {
        throw new NotImplementedException();
    }
}