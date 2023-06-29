using Cyberstar.AssetManagement;
using Cyberstar.Logging;

namespace Cyberstar.Scenes;

public class SceneManager : Scene
{
    public bool ApplicationCloseRequested { get; set; }
    public Scene ActiveScene;
    
    public SceneManager(ILogger logger, AssetManager assets) : base(logger, assets)
    {
    }

    public void BeginLoadActiveScene(Scene scene)
    {
        ActiveScene = scene;
    }

    public override void ReadInput()
    {
        ActiveScene.ReadInput();
    }

    public override void UpdateState()
    {
        ActiveScene.UpdateState();
    }

    public override void Render()
    {
        ActiveScene.Render();
    }
}