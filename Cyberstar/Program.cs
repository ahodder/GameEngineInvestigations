using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.Logging;
using Cyberstar.Scenes;
using Raylib_cs;

Raylib.InitWindow(1200, 800, "Hello, world");
// Raylib.SetWindowMonitor(1);

// var texture = Raylib.LoadTexture("assets/textures/atlas/dev_ships.png");

var logger = new FileLogger("Logs.txt");
var assets = new AssetManager();
var sceneManager = new SceneManager(logger, assets);
sceneManager.BeginLoadActiveScene(new MainMenuScene(sceneManager, logger, assets));

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

