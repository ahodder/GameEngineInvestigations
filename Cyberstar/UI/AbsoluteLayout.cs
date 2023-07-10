using System.Drawing;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Raylib_cs;

namespace Cyberstar.UI;

public class AbsoluteLayout : ViewBase
{
    private readonly List<Child> Children = new List<Child>();
    
    public AbsoluteLayout(AssetManager assetManager) : base(assetManager)
    {
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        var totalWidth = 0;
        var totalHeight = 0;

        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.View.Measure(child.LayoutParams.X, child.LayoutParams.Y, child.LayoutParams.Width, child.LayoutParams.Height);
            totalWidth = Math.Max(totalWidth, child.LayoutParams.X + child.LayoutParams.Width);
            totalHeight = Math.Max(totalHeight, child.LayoutParams.Y + child.LayoutParams.Height);
        }

        return new Point(totalWidth, totalHeight);
    }

    protected override void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.View.Render(in frameTiming, in inputData);
        }
    }

    public void AddView(IView child, int x, int y)
    {
        child.Measure(x, y, -1, -1);
        AddView(child, x, y, child.MeasuredSize.X, child.MeasuredSize.Y);
    }

    public void AddView(IView child, int x, int y, int width, int height)
    {
        Children.Add(new Child(child, new LayoutParams(x, y, width, height)));
    }

    public override void HandleKeyboardKeys(in FrameTiming frameTiming, Span<KeyboardKey> keys)
    {
        foreach (var child in Children)
        {
            child.View.HandleKeyboardKeys(in frameTiming, keys);
        }
    }

    public override bool WillHandleMouseClick(int mouseX, int mouseY)
    {
        foreach (var child in Children)
            if (child.View.WillHandleMouseClick(mouseX, mouseY))
                return true;

        return false;
    }

    private readonly struct Child
    {
        public readonly IView View;
        public readonly LayoutParams LayoutParams;

        public Child(IView view, LayoutParams layoutParams)
        {
            View = view;
            LayoutParams = layoutParams;
        }
    }

    private readonly struct LayoutParams
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public LayoutParams(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}