using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Cyberstar.Game.Systems;
using Cyberstar.Logging;
using Raylib_cs;

namespace Cyberstar.Scenes;

public class TestoScene : Scene
{
    private Entity _player;
    
    public TestoScene(ILogger logger, AssetManager assets) : base(logger, assets)
    {
        Create();
    }
    
    public override void PerformTick(FrameTiming frameTiming)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_M))
        {
            using (var file = File.OpenWrite("TestSerialization.blob"))
            {
                EntityManager.Serialize(file);
                Log.Information("Saved entity manager to disk");
            }
        }

        if (Raylib.IsKeyPressed(KeyboardKey.KEY_N))
        {
            using (var file = File.OpenRead("TestSerialization.blob"))
            {
                EntityManager.Deserialize(file);
                Log.Information("Loaded entity manager from disk");
            }
        }
        
        // Read player input

        EntityManager.RunSystems(frameTiming);
    }

    public void Create()
    {
        // Register Systems
        EntityManager.AddSystem(new MainPlayerShipInputSystem());
        EntityManager.AddSystem(new ShipMovementSystem());
        EntityManager.AddSystem(new SpriteRenderingSystem(Assets));
        
        _player = EntityManager.CreateEntity();
        EntityManager.SetComponentsFor(_player,
            new MainPlayerComponent(),
            new ShipComponent
            {
                Acceleration = 1,
                MaximumVelocity = 5,
                RotationalAcceleration = 0.5f,
                MaximumRotationalVelocity = 2.5f,
            },
            new ShipControlComponent(),
            new VelocityComponent
            {
                Velocity = Vector2.Zero,
                Drag = 0.001f,
            },
            new SpriteComponent
            {
                SpriteAtlas = "dev_ships",
                SpriteAnimationPath = "red_ship",
                SpriteCurrentFrame = 0,
            },
            TransformComponent.FromTranslation(new Vector2(200, 200)));
    }
}