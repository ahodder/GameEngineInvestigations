using System.Drawing;
using Cyberstar.Core;
using Cyberstar.Engine.AssetManagement;
using Raylib_cs;

namespace Cyberstar.Engine.UI;

public class AbsoluteLayout : ViewParent
{
    public AbsoluteLayout(AssetManager assetManager) : base(assetManager)
    {
    }

    protected override Point MeasureSelf(int x, int y, int width, int height)
    {
        var totalWidth = 0;
        var totalHeight = 0;

        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.MeasureAndLayout(child.Bounds.X, child.Bounds.Y, child.Bounds.Width, child.Bounds.Height);
            totalWidth = Math.Max(totalWidth, child.Bounds.X + child.Bounds.Width);
            totalHeight = Math.Max(totalHeight, child.Bounds.Y + child.Bounds.Height);
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

    public void AddView(IView child, int x, int y)
    {
        child.MeasureAndLayout(x, y, -1, -1);
        AddView(child, x, y, child.MeasuredSize.X, child.MeasuredSize.Y);
    }

    public void AddView(IView child, int x, int y, int width, int height)
    {
        child.RequestedSize = new Point(width, height);
        child.MeasureAndLayout(x, y, width, height);
        AddChild(child);
    }

    public override void HandleKeyboardKeys(in FrameTiming frameTiming, Span<KeyboardKey> keys)
    {
        foreach (var child in Children)
        {
            child.HandleKeyboardKeys(in frameTiming, keys);
        }
    }
}