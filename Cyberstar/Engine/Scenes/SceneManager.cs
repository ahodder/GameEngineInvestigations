using Cyberstar.Core;

namespace Cyberstar.Engine.Scenes;

public class SceneManager : Scene
{
    public bool ApplicationCloseRequested { get; set; }
    public Scene? ActiveScene;
    
    public SceneManager(IEngine engine) : base(engine)
    {
    }

    public void BeginLoadActiveScene(Scene scene)
    {
        if (ActiveScene != null)
            ActiveScene.OnSceneUnloaded();
        ActiveScene = scene;
        ActiveScene.OnSceneLoaded();
    }

    public override void PerformTick(FrameTiming frameTiming)
    {
        ActiveScene!.PerformTick(frameTiming);
    }
}