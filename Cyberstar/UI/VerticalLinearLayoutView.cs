using System.Drawing;
using Cyberstar.AssetManagement;

namespace Cyberstar.UI;

public class VerticalLinearLayoutView : ViewBase
{
    public List<IView> Children { get; } = new List<IView>();

    public int ViewSpacing { get; set; }

    public VerticalLinearLayoutView(AssetManager assetManager) : base(assetManager)
    {
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        var totalWidth = 0;
        var totalHeight = 0;

        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.Measure(x, y, width, height);
            var dh = child.Bounds.Height + ViewSpacing;
            y += dh;
            totalHeight += dh;
            totalWidth = System.Math.Max(child.Bounds.Width, totalWidth);
        }

        return new Point(totalWidth, totalHeight);
    }

    protected override void DoRenderContent()
    {
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.Render();
        }
    }

    public VerticalLinearLayoutView AddView(IView view)
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