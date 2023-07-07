namespace Cyberstar.UI;

public readonly struct InputData
{
    public readonly int MouseX;
    public readonly int MouseY;

    public InputData(int mouseX, int inputY)
    {
        MouseX = mouseX;
        MouseY = inputY;
    }
}