namespace Cyberstar.UI;

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
    
    public int Width => Left + Right;
    public int Height => Top + Bottom;

    public Thickness Set(int allSides)
    {
        Left = allSides;
        Top = allSides;
        Right = allSides;
        Bottom = allSides;
        return this;
    }
    
    public Thickness Set(int horizontal, int vertical)
    {
        Left = horizontal;
        Top = vertical;
        Right = horizontal;
        Bottom = vertical;
        return this;
    }
    
    public Thickness Set(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
        return this;
    }
}