using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Raylib_cs;

namespace Cyberstar.UI;

public abstract class ViewParent : ViewBase
{
    public abstract IReadOnlyList<IView> Children { get; }

    protected ViewParent(AssetManager assetManager) : base(assetManager)
    {
    }

    public override void HandleKeyboardKeys(in FrameTiming frameTiming, Span<KeyboardKey> keys)
    {
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.HandleKeyboardKeys(frameTiming, keys);
        }
    }

    public override void ClearFocus()
    {
        base.ClearFocus();
        for (var i = 0; i < Children.Count; i++)
            Children[i].ClearFocus();
    }
    
    public override bool WillHandleMouseClick(int mouseX, int mouseY)
    {
        var ret = false;
        
        foreach (var child in Children)
        {
            child.ClearFocus();
            if (!ret && child.WillHandleMouseClick(mouseX, mouseY))
                ret = true;
        }

        return ret;
    }
}