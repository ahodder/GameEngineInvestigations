using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Cyberstar.Game.Systems;
using Cyberstar.Logging;

namespace Cyberstar.Scenes;

public class TestoScene : Scene
{
    
    
    public TestoScene(ILogger logger, AssetManager assets) : base(logger, assets)
    {
        Create();
    }
    
    public override void PerformTick(FrameTiming frameTiming)
    {
        EntityManager.RunSystems(frameTiming);
    }

    public void Create()
    {
        // Register Systems
        EntityManager.AddSystem(new SpriteRenderingSystem(Assets));
        
        var sprite = new SpriteComponent
        {
            SpriteAtlas = "dev_ships",
            SpriteAnimationPath = "red_ship",
            SpriteCurrentFrame = 0,
        };

        var transform = new TransformComponent
        {
            X = 50,
            Y = 100,
        };
        
        var player = EntityManager.CreateEntity();
        EntityManager.SetComponentsFor(player, sprite, transform);
    }
}