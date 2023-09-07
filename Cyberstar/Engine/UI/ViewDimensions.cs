using System.Drawing;
using System.Numerics;
using Rectangle = Raylib_cs.Rectangle;

namespace Cyberstar.Engine.UI;

public struct ViewDimensions
{
    public Thickness Margin;
    public Thickness Padding;
    public Point Position;
    public Point ContentSize;
    
    public int ContentStartX => Margin.Left + Padding.Left + Position.X;
    public int ContentStartY => Margin.Top + Padding.Top + Position.Y;

    public int ContentWidth => ContentSize.X;
    public int ContentHeight => ContentSize.Y;

    public int PaddedStartX => Position.X + Margin.Left;
    public int PaddedStartY => Position.Y + Margin.Top;

    public int PaddedWidth => ContentSize.X + Padding.Width;
    public int PaddedHeight => ContentSize.Y + Padding.Height;

    public int ActualX => Position.X;
    public int ActualY => Position.Y;
    public int ActualWidth => Margin.Width + Padding.Width + ContentSize.X;
    public int ActualHeight => Margin.Height + Padding.Height + ContentSize.Y;

    public int Bottom => ActualY + ActualHeight;

    public ViewDimensions(int x, int y, int width, int height)
    {
        Margin = Thickness.Zero;
        Padding = Thickness.Zero;
        Position = new Point(x, y);
        ContentSize = new Point(width, height);
    }

    public bool Contains(Vector2 position)
    {
        var x = position.X;
        var y = position.Y;

        return x >= PaddedStartX && x < PaddedStartX + PaddedWidth &&
               y >= PaddedStartY && y < PaddedStartY + PaddedHeight;
    }
}