using Cyberstar.AssetManagement;

namespace Cyberstar.UI;

public class UiRenderer
{
    public IView? RootView { get; set; }
    
    private readonly AssetManager _assetManager;
    
    public UiRenderer(AssetManager assetManager, IView? rootView)
    {
        _assetManager = assetManager;
        RootView = rootView;
    }

    public void Render()
    {
        RootView?.Render(_assetManager);
    }
}