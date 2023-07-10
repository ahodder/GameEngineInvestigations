using System.Drawing;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Raylib_cs;

namespace Cyberstar.UI;

public class VerticalLayoutView : ViewParent
{
    public override IReadOnlyList<IView> Children => _children;
    
    public int ViewSpacing { get; set; }

    private readonly List<IView> _children = new List<IView>();

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
            totalWidth = Math.Max(child.Bounds.Width, totalWidth);
        }

        return new Point(totalWidth, totalHeight);
    }

    protected override void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.Render(in frameTiming, in inputData);
        }
    }

    public VerticalLayoutView AddView(IView view)
    {
        _children.Add(view);
        return this;
    }

    public override void HandleKeyboardKeys(in FrameTiming frameTiming, Span<KeyboardKey> keys)
    {
        foreach (var child in Children)
        {
            child.HandleKeyboardKeys(frameTiming, keys);
        }
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