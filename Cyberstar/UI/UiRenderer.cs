using Cyberstar.AssetManagement;
using Raylib_cs;

namespace Cyberstar.UI;

public class UiRenderer
{
    public IView RootView { get; set; }
    
    public UiRenderer(IView rootView, int x, int y, int width, int height)
    {
        RootView = rootView;
        RootView.Measure(x, y, width, height);
    }

    public void Render()
    {
        RootView.Render();
        var mousePosition = Raylib.GetMousePosition();
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            RootView.WillHandleMouseClick((int)mousePosition.X, (int)mousePosition.Y);
    }
}