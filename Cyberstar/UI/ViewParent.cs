using Cyberstar.Core;
using Cyberstar.Engine.AssetManagement;
using Raylib_cs;

namespace Cyberstar.UI;

public abstract class ViewParent : ViewBase
{
    public IReadOnlyList<IView> Children => _children;
    
    private readonly List<IView> _children = new List<IView>();

    protected ViewParent(AssetManager assetManager) : base(assetManager)
    {
    }

    protected void AddChild(IView child)
    {
        _children.Add(child);
        child.Parent = this;
    }

    protected void RemoveChild(IView child)
    {
        _children.Remove(child);
        child.Parent = null;
    }

    public void RemoveAllChildren()
    {
        foreach (var child in _children)
        {
            child.Parent = null;
        }
        
        _children.Clear();
    }

    public bool TryFindViewByTag(ReadOnlySpan<char> tag, out IView? foundView)
    {
        if (tag.Equals(Tag, StringComparison.Ordinal))
        {
            foundView = this;
            return true;
        }

        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            if (child is ViewParent localParent && localParent.TryFindViewByTag(tag, out foundView))
            {
                return true;
            }
            else if (tag.Equals(child.Tag, StringComparison.Ordinal))
            {
                foundView = child;
                return true;
            }   
        }

        foundView = null;
        return false;
    }

    protected override void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.Render(in frameTiming, in inputData);
        }
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