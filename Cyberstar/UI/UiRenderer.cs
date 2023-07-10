using Cyberstar.Core;
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

    public void Render(in FrameTiming frameTiming)
    {
        var mousePosition = Raylib.GetMousePosition();
        var inputData = new InputData((int)mousePosition.X, (int)mousePosition.Y);

        Span<KeyboardKey> keys = stackalloc KeyboardKey[128];
        var cnt = 0;
        var key = Raylib.GetKeyPressed();
        while (key != 0)
        {
            keys[cnt++] = (KeyboardKey)key;
            key = Raylib.GetKeyPressed();
        }

        RootView.HandleKeyboardKeys(in frameTiming, keys.Slice(0, cnt));

        RootView.Render(in frameTiming, in inputData);
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            RootView.WillHandleMouseClick(inputData.MouseX, inputData.MouseY);
    }
}