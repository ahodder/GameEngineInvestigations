using System.Numerics;
using Cyberstar.UI;
using Raylib_cs;

Raylib.InitWindow(1200, 800, "Hello, world");
Raylib.SetWindowMonitor(1);

var texture = Raylib.LoadTexture("assets/textures/atlas/dev_ships.png");

var atlas = new FontAtlas("assets/fonts/TimesNewRoman.ttf");
var renderer = new UiRenderer(atlas);
var button = new Button(atlas.DefaultFont, "Button!! n", 250, 250, fontSize: 25);
button.Background.BackgroundColor = Color.GOLD;
button.Dimensions.Padding.Set(5);
button.Dimensions.Margin.Set(10);
button.OnClick = () =>
{
    Console.WriteLine("Howdy, Sailor");
};

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
    Raylib.DrawTextEx(atlas.DefaultFont, "Hello, world!", new Vector2(12, 12), 20, 1, Color.RED);
    renderer.DrawButton(button);
}