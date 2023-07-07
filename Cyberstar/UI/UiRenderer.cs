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
        var mousePosition = Raylib.GetMousePosition();
        var inputData = new InputData((int)mousePosition.X, (int)mousePosition.Y);
        RootView.Render(in inputData);
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            RootView.WillHandleMouseClick(inputData.MouseX, inputData.MouseY);
    }
}