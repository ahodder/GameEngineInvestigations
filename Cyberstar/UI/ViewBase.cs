using System.Drawing;
using Cyberstar.AssetManagement;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Rectangle = System.Drawing.Rectangle;

namespace Cyberstar.UI;

public abstract class ViewBase : IView
{
    public bool IsEnabled { get; set; }
    public Rectangle Bounds { get; private set; }
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
        ContentBounds = new Rectangle(Bounds.X + Margin.Left + Padding.Left, Bounds.Y + Margin.Top + Padding.Top, MeasuredSize.X, MeasuredSize.Y);
        ;
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

    public void Render(in InputData inputData)
    {
        if (!IsEnabled) return;
        
        Raylib.DrawRectangle(Bounds.Left + Margin.Left, Bounds.Top + Margin.Top, Bounds.Width - Margin.Width, Bounds.Height - Margin.Height, BackgroundColor);

        Raylib.BeginScissorMode(ContentBounds.X, ContentBounds.Y, ContentBounds.Width, ContentBounds.Height); 
        DoRenderContent(in inputData);
        Raylib.EndScissorMode();
    }

    /// <summary>
    /// Renders only the content of the view.
    /// </summary>
    protected abstract void DoRenderContent(in InputData inputData);

    public virtual bool WillReceiveFocus(int x, int y)
    {
        return false;
    }

    public virtual bool WillHandleMouseClick(int mouseX, int mouseY)
    {
        return false;
    }
}