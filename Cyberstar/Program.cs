using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Logging;
using Cyberstar.Scenes;
using Cyberstar.UI;
using Raylib_cs;

Raylib.InitWindow(1200, 800, "Hello, world");
// Raylib.SetWindowMonitor(1);

// var texture = Raylib.LoadTexture("assets/textures/atlas/dev_ships.png");

var logger = new FileLogger("Logs.txt");
var assets = new AssetManager();
var activeScene = new MainMenuScene(logger, assets);

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.BLACK);
    
    activeScene.ReadInput();
    activeScene.UpdateState();
    activeScene.Render();
    
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

