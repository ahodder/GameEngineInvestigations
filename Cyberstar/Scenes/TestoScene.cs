using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Cyberstar.Game.Systems;
using Cyberstar.Logging;
using Cyberstar.Strings;
using Cyberstar.UI;
using Raylib_cs;

namespace Cyberstar.Scenes;

public class TestoScene : Scene
{
    private UiRenderer _uiRenderer;
    private LabelView _fpsCounter;
    
    private Entity _player;
    
    public TestoScene(ILogger logger, WindowData windowData, AssetManager assets) : base(logger, windowData, assets)
    {
        CreateUi();
        CreateWorld();
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
        
        EntityManager.RunSystems(frameTiming);
        
        var sb = new ValueStringBuilder(stackalloc char[64]);
        sb.Append("FPS: ");
        sb.Append((int)frameTiming.Fps);
        _fpsCounter.SetText(sb.AsSpan());

        _uiRenderer.Render();
    }

    public void CreateUi()
    {
        _fpsCounter = new LabelView(Assets);
        _fpsCounter.FontSize = 26;
        _fpsCounter.TextColor = Color.GOLD;
        _fpsCounter.BackgroundColor = Color.BLUE;
        _fpsCounter.Padding = new Thickness().Set(10);
        _fpsCounter.Text = "Something";
        
        var layout = new VerticalLayoutView(Assets);
        layout.AddView(_fpsCounter);

        _uiRenderer = new UiRenderer(layout, 0, 0, 1000, 1000);
    }

    public void CreateWorld()
    {
        Assets.TryLoadSpriteAtlas("dev_ships", out var atlas);
        
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