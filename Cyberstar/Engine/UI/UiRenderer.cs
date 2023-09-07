using Cyberstar.Core;
using Raylib_cs;

namespace Cyberstar.Engine.UI;

public class UiRenderer
{
    public IView RootView { get; set; }

    private int _x;
    private int _y;
    private int _width;
    private int _height;
    
    public UiRenderer(IView rootView, int x, int y, int width, int height)
    {
        RootView = rootView;
        RootView.MeasureAndLayout(x, y, width, height);
        _x = x;
        _y = y;
        _width = width;
        _height = height;
    }

    public void Render(in FrameTiming frameTiming)
    {
        if (RootView.NeedsRemeasure)
            RootView.MeasureAndLayout(_x, _y, _width, _height);

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