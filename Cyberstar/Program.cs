// See https://aka.ms/new-console-template for more information


using Cyberstar.ECS;
using Raylib_cs;

Raylib.InitWindow(1200, 800, "Hello, world");
Raylib.SetWindowMonitor(1);

var texture = Raylib.LoadTexture("assets/textures/atlas/dev_ships.png");

// var world = new World();

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.BLACK);
    
    UpdateGame();
    PerformPhysics();
    RenderGame();
    DrawUi();
    
    Raylib.EndDrawing();
}

Raylib.CloseWindow();




void UpdateGame()
{
}

void PerformPhysics()
{
}

void RenderGame()
{
    Raylib.DrawTexture(texture, 0, 0, Color.WHITE);
}

void DrawUi()
{
    Raylib.DrawText("Hello, world!", 12, 12, 20, Color.RED);
}