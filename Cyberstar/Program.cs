using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.Logging;
using Cyberstar.Scenes;
using Raylib_cs;

var windowData = new WindowData("Cyberstar", 1200, 800);
Raylib.InitWindow(windowData.Width, windowData.Height, windowData.WindowName);

var logger = Log.Instance = new ConsoleLogger();
var assets = new AssetManager(logger, "assets");
var sceneManager = new SceneManager(logger,windowData, assets);

// sceneManager.BeginLoadActiveScene(new MainMenuScene(sceneManager, logger, windowData, assets));
sceneManager.BeginLoadActiveScene(new ShipBuilderScene(logger, windowData, assets));

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

