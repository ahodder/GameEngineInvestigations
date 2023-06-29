using Cyberstar.AssetManagement;
using Cyberstar.Core;
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

    public override void PerformTick(FrameTiming frameTiming)
    {
        ActiveScene.PerformTick(frameTiming);
    }
}