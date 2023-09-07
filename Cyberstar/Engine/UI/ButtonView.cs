using Cyberstar.Engine.AssetManagement;

namespace Cyberstar.Engine.UI;

public class ButtonView : LabelView
{
    public Action? OnClick { get; set; }

    public ButtonView(AssetManager assetManager) : base(assetManager)
    {
    }

    public override bool WillHandleMouseClick(int mouseX, int mouseY)
    {
        if (PaddedBounds.Contains(mouseX, mouseY))
        {
            OnClick?.Invoke();
            return true;
        }

        return false;
    }
}