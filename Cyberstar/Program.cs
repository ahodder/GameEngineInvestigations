using Cyberstar.Core;
using Cyberstar.Engine;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.Engine.Logging;
using Cyberstar.Game.Scenes;
using Raylib_cs;

var windowData = new WindowData("Cyberstar", 1200, 800);
Raylib.InitWindow(windowData.Width, windowData.Height, windowData.WindowName);
// Raylib.SetTargetFPS(60);

var logger = Log.Instance = new ConsoleLogger();
var assets = new AssetManager(logger, "assets");
var engine = new SimpleEngine(logger, assets, windowData);

engine.SceneManager.BeginLoadActiveScene(new MainMenuScene(engine));

FrameTiming ft = new FrameTiming();
while (!Raylib.WindowShouldClose() && !engine.SceneManager.ApplicationCloseRequested)
{
    ft.Fps = Raylib.GetFPS();
    ft.DeltaTime = Raylib.GetFrameTime();
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.BLACK);
    
    engine.AudioManager.PerformTick(ft);
    
    engine.SceneManager.PerformTick(ft);
    
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

