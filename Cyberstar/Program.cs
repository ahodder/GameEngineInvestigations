using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.Logging;
using Cyberstar.Scenes;
using Raylib_cs;

Raylib.InitWindow(1200, 800, "Hello, world");

var logger = Log.Instance = new ConsoleLogger();
var assets = new AssetManager(logger, "assets");
var sceneManager = new SceneManager(logger, assets);

var loadTexture = assets.TryLoadSpriteAtlas("dev_ships", out var atlas);

sceneManager.BeginLoadActiveScene(new MainMenuScene(sceneManager, logger, assets));
// sceneManager.BeginLoadActiveScene(new ShipBuilderScene(logger, assets));

FrameTiming ft = new FrameTiming();
while (!Raylib.WindowShouldClose() && !sceneManager.ApplicationCloseRequested)
{
    ft.Fps = Raylib.GetFPS();
    ft.DeltaTime = Raylib.GetFrameTime();
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.BLACK);
    
    sceneManager.PerformTick(ft);
    
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

