using System.Drawing;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Rectangle = System.Drawing.Rectangle;

namespace Cyberstar.UI;

public abstract class ViewBase : IView
{
    public bool IsEnabled { get; set; }
    public Rectangle Bounds { get; private set; }
    public Rectangle PaddedBounds { get; private set; }
    public Rectangle ContentBounds { get; private set; }
    public Thickness Margin { get; set; }
    public Thickness Padding { get; set; }
    public Point MeasuredSize { get; protected set; }
    public Point RequestedSize { get; set; }
    public Color BackgroundColor { get; set; }
    
    protected AssetManager AssetManager { get; }

    public ViewBase(AssetManager assetManager)
    {
        IsEnabled = true;
        AssetManager = assetManager;
        BackgroundColor = Color.BLACK;
    }

    public void Measure(int x, int y, int width, int height)
    {
        if (RequestedSize == Point.Empty)
            MeasuredSize = DoMeasure(x, y, width, height);
        else
            MeasuredSize = RequestedSize;
        
        Bounds = new Rectangle(x, y, Margin.Width + Padding.Width + MeasuredSize.X, Margin.Height + Padding.Height + MeasuredSize.Y);
        PaddedBounds = new Rectangle(Bounds.Left + Margin.Left, Bounds.Top + Margin.Top, Bounds.Width - Margin.Width, Bounds.Height - Margin.Height);
        ContentBounds = new Rectangle(Bounds.X + Margin.Left + Padding.Left, Bounds.Y + Margin.Top + Padding.Top, MeasuredSize.X, MeasuredSize.Y);
    }

    /// <summary>
    /// Measures the contents of the view.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    protected abstract Point DoMeasure(int x, int y, int width, int height);

    public void Render(in FrameTiming frameTiming, in InputData inputData)
    {
        if (!IsEnabled || Bounds.Width == 0 || Bounds.Height == 0) return;
        
        Raylib.DrawRectangle(Bounds.Left + Margin.Left, Bounds.Top + Margin.Top, Bounds.Width - Margin.Width, Bounds.Height - Margin.Height, BackgroundColor);

        Raylib.BeginScissorMode(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height); 
        DoRenderContent(in frameTiming, in inputData);
        Raylib.EndScissorMode();
    }

    /// <summary>
    /// Renders only the content of the view.
    /// </summary>
    protected abstract void DoRenderContent(in FrameTiming frameTiming, in InputData inputData);

    public virtual bool WillReceiveFocus(int x, int y)
    {
        return false;
    }

    public virtual void HandleKeyboardKeys(in FrameTiming frameTiming, Span<KeyboardKey> keys)
    {
    }

    public virtual bool WillHandleMouseClick(int mouseX, int mouseY)
    {
        return false;
    }
}