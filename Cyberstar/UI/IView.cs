using System.Drawing;


namespace Cyberstar.UI;

public interface IView
{
    /// <summary>
    /// Whether or not the view should render (is the view active in the view hierarchy).
    /// </summary>
    bool IsEnabled { get; set; }
    
    /// <summary>
    /// The real-space complete bounds of the view.
    /// </summary>
    Rectangle Bounds { get; }

    /// <summary>
    /// The real-space bounds of the view less the margin and padding.
    /// </summary>
    Rectangle ContentBounds { get; }
    
    /// <summary>
    /// The extra space around the view with respect the view's parent.
    /// </summary>
    Thickness Margin { get; set; }
    /// <summary>
    /// The padding of the view's content.
    /// </summary>
    Thickness Padding { get; set; }
    
    /// <summary>
    /// The measured size of the view.
    /// </summary>
    Point MeasuredSize { get; }
    
    /// <summary>
    /// The requested size of the view. If this is nonzero, then the view will always be this size when measured.
    /// </summary>
    Point RequestedSize { get; }
    
    /// <summary>
    /// The background color of the view. This is drawn before the background texture.
    /// </summary>
    Raylib_cs.Color BackgroundColor { get; set; }
    
    /// <summary>
    /// Requests that the view measure itself to fit the smallest area possible.
    /// </summary>
    /// <param name="x">The offset within the real space that the view is measured against.</param>
    /// <param name="y">The offset within the real space that the view is measured against.</param>
    /// <param name="width">The maximum width of the view.</param>
    /// <param name="height">The maximum height of the view.</param>
    void Measure(int x, int y, int width, int height);

    /// <summary>
    /// Renders the view to the given dimensions.
    /// </summary>
    void Render(in InputData inputData);

    /// <summary>
    /// Determines whether or not the view will handle the mouse given mouse event.
    /// </summary>
    /// <param name="mouseX"></param>
    /// <param name="mouseY"></param>
    /// <returns></returns>
    bool WillHandleMouseClick(int mouseX, int mouseY);
}