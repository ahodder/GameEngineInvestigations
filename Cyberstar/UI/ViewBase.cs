using System.Drawing;
using Cyberstar.Core;
using Cyberstar.Engine.AssetManagement;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Rectangle = System.Drawing.Rectangle;

namespace Cyberstar.UI;

public abstract class ViewBase : IView
{
    public string Tag { get; set; }
    public bool HasFocus { get; protected set; }
    public bool IsEnabled { get; set; }
    public ViewParent? Parent { get; set; }

    public bool NeedsRemeasure
    {
        get => _needsRemeasure;
        set
        {
            _needsRemeasure = value;
            if (Parent != null)
                Parent.NeedsRemeasure = value;
        }
    }
    public Rectangle Bounds { get; private set; }
    public Rectangle PaddedBounds { get; private set; }
    public Rectangle ContentBounds { get; private set; }
    public Thickness Margin { get; set; }
    public Thickness Padding { get; set; }
    public Point MeasuredSize { get; protected set; }
    public Point RequestedSize { get; set; }
    public Color? BackgroundColor { get; set; }
    
    protected AssetManager AssetManager { get; }

    private bool _needsRemeasure;

    public ViewBase(AssetManager assetManager)
    {
        IsEnabled = true;
        AssetManager = assetManager;
    }

    public void Measure(int x, int y, int width, int height)
    {
        NeedsRemeasure = false;

        MeasuredSize = DoMeasure(x, y, width, height);
        if (RequestedSize != Point.Empty)
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

        if (BackgroundColor != null)
        {
            Raylib.DrawRectangle(Bounds.Left + Margin.Left, Bounds.Top + Margin.Top, Bounds.Width - Margin.Width, Bounds.Height - Margin.Height, BackgroundColor.Value);
        }

        // Raylib.BeginScissorMode(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height); 
        DoRenderContent(in frameTiming, in inputData);
        // Raylib.EndScissorMode();
    }

    /// <summary>
    /// Renders only the content of the view.
    /// </summary>
    protected abstract void DoRenderContent(in FrameTiming frameTiming, in InputData inputData);

    public virtual void ClearFocus()
    {
        HasFocus = false;
    }

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