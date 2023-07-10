using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Raylib_cs;

namespace Cyberstar.UI;

public abstract class ViewParent : ViewBase
{
    public List<IView> Children { get; protected set; } = new List<IView>();
    
    protected ViewParent(AssetManager assetManager) : base(assetManager)
    {
    }

    public override void HandleKeyboardKeys(in FrameTiming frameTiming, Span<KeyboardKey> keys)
    {
        foreach (var child in Children)
        {
            child.HandleKeyboardKeys(frameTiming, keys);
        }
    }
}