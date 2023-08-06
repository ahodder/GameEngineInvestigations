using System.Drawing;
using Cyberstar.Core;
using Cyberstar.Engine.AssetManagement;
using Raylib_cs;

namespace Cyberstar.UI;

public class HorizontalLayoutView : ViewParent
{
    public int ViewSpacing { get; set; }

    public HorizontalLayoutView(AssetManager assetManager) : base(assetManager)
    {
    }

    protected override Point MeasureSelf(int x, int y, int width, int height)
    {
        var totalWidth = 0;
        var maxHeight = 0;
        
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.MeasureAndLayout(x + totalWidth, y, width, height);
            totalWidth += child.Bounds.Width + ViewSpacing;
            maxHeight = Math.Max(child.Bounds.Height, maxHeight);
        }

        return new Point(totalWidth, maxHeight);
    }

    public void AddView(IView view)
    {
        AddChild(view);
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