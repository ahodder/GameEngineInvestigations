using System.Drawing;
using Cyberstar.AssetManagement;

namespace Cyberstar.UI;

public class VerticalLayoutView : ViewBase
{
    public List<IView> Children { get; } = new List<IView>();

    public int ViewSpacing { get; set; }

    public VerticalLayoutView(AssetManager assetManager) : base(assetManager)
    {
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        var totalWidth = 0;
        var totalHeight = y;
        
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.Measure(x, totalHeight, width, height);
            totalHeight += child.Bounds.Height + ViewSpacing;
            totalWidth = System.Math.Max(child.Bounds.Width, totalWidth);
        }

        return new Point(totalWidth, totalHeight);
    }

    protected override void DoRenderContent(in InputData inputData)
    {
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.Render(in inputData);
        }
    }

    public VerticalLayoutView AddView(IView view)
    {
        Children.Add(view);
        return this;
    }

    public override bool WillHandleMouseClick(int mouseX, int mouseY)
    {
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            if (child.WillHandleMouseClick(mouseX, mouseY))
                return true;
        }

        return false;
    }
}