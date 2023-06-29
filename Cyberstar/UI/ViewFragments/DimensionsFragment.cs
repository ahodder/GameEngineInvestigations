using System.Drawing;
using System.Numerics;

namespace Cyberstar.UI.ViewFragments;

public struct Thickness
{
    public static readonly Thickness Zero = new Thickness
    {
        Left = 0,
        Top = 0,
        Right = 0,
        Bottom = 0,
    };
    
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public void Set(int allSides)
    {
        Left = allSides;
        Top = allSides;
        Right = allSides;
        Bottom = allSides;
    }
    
    public void Set(int horizontal, int vertical)
    {
        Left = horizontal;
        Top = vertical;
        Right = horizontal;
        Bottom = vertical;
    }
    
    public void Set(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public int Width => Left + Right;
    public int Height => Top + Bottom;
}

public struct DimensionsFragment
{
    public Thickness Margin;
    public Thickness Padding;
    public Rectangle Bounds;

    public int ContentStartX => Bounds.X;
    public int ContentStartY => Bounds.Y;

    public int ContentWidth => Bounds.Width;
    public int ContentHeight => Bounds.Height;

    public int PaddedStartX => Bounds.X - Padding.Left;
    public int PaddedStartY => Bounds.Y - Padding.Top;

    public int PaddedWidth => Padding.Width + Bounds.Width;
    public int PaddedHeight => Padding.Height + Bounds.Height;

    public int ActualX => Bounds.X - Padding.Left - Margin.Left;
    public int ActualY => Bounds.Y - Padding.Top - Margin.Top;
    public int ActualWidth => Margin.Width + Padding.Width + Bounds.Width;
    public int ActualHeight => Margin.Height + Padding.Height + Bounds.Height;

    public DimensionsFragment(int x, int y, int width, int height)
    {
        Margin = Thickness.Zero;
        Padding = Thickness.Zero;
        Bounds = new Rectangle(x, y, width, height);
    }

    public bool Contains(Vector2 position)
    {
        var x = position.X;
        var y = position.Y;

        return x >= PaddedStartX && x < PaddedStartX + PaddedWidth &&
               y >= PaddedStartY && y < PaddedStartY + PaddedHeight;
    }
}