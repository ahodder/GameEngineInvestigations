namespace Cyberstar.Core;

public struct WindowData
{
    public string WindowName;
    public int Width;
    public int Height;

    public WindowData(ReadOnlySpan<char> windowName, int width, int height)
    {
        WindowName = new string(windowName);
        Width = width;
        Height = height;
    }
}