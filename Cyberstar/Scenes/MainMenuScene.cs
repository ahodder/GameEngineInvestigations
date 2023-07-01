using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.Logging;
using Cyberstar.UI;
using Raylib_cs;

namespace Cyberstar.Scenes;

public class MainMenuScene : Scene
{
    public Button Continue;
    public Button NewGame;
    public Button LoadGame;
    public Button Settings;
    public Button Exit;

    private SceneManager _sceneManager;
    private UiRenderer _uiRenderer;

    public MainMenuScene(SceneManager sceneManager, ILogger logger, AssetManager assets) : base(logger, assets)
    {
        var font = assets.FontAtlas.DefaultFont;
        var fontSize = 25;
        Continue = new Button(font, "Continue", 100, 50, fontSize);
        Continue.Background.BackgroundColor = Color.GRAY;
        Continue.Dimensions.Padding.Set(5);
        Continue.Dimensions.Margin.Set(10);
        Continue.OnClick += OnContinueClicked;
        
        NewGame = new Button(font, "New Game", 100, Continue.Dimensions.Bottom, fontSize);
        NewGame.Background.BackgroundColor = Color.GRAY;
        NewGame.Dimensions.Padding.Set(5);
        NewGame.Dimensions.Margin.Set(10);
        NewGame.OnClick += OnNewGameClicked;
        
        LoadGame = new Button(font, "Load Game", 100, NewGame.Dimensions.Bottom, fontSize);
        LoadGame.Background.BackgroundColor = Color.GRAY;
        LoadGame.Dimensions.Padding.Set(5);
        LoadGame.Dimensions.Margin.Set(10);
        LoadGame.OnClick += OnLoadGameClicked;
        
        Settings = new Button(font, "Settings", 100, LoadGame.Dimensions.Bottom, fontSize);
        Settings.Background.BackgroundColor = Color.GRAY;
        Settings.Dimensions.Padding.Set(5);
        Settings.Dimensions.Margin.Set(10);
        Settings.OnClick += OnSettingsClicked;
        
        Exit = new Button(font, "Exit", 100, Settings.Dimensions.Bottom, fontSize);
        Exit.Background.BackgroundColor = Color.GRAY;
        Exit.Dimensions.Padding.Set(5);
        Exit.Dimensions.Margin.Set(10);
        Exit.OnClick += OnExitClicked;

        _sceneManager = sceneManager;
        _uiRenderer = new UiRenderer(assets.FontAtlas);
    }

    public override void PerformTick(FrameTiming frameTiming)
    {
        RenderUi();
    }

    
    public void RenderUi() 
    {
        _uiRenderer.DrawButton(Continue);
        _uiRenderer.DrawButton(NewGame);
        _uiRenderer.DrawButton(LoadGame);
        _uiRenderer.DrawButton(Settings);
        _uiRenderer.DrawButton(Exit);
    }

    public void OnContinueClicked()
    {
        Console.WriteLine("Continue Clicked");
    }
    
    public void OnNewGameClicked()
    {
        _sceneManager.BeginLoadActiveScene(new TestoScene(_logger, Assets));
    }
    
    public void OnLoadGameClicked()
    {
        Console.WriteLine("Load Game Clicked");
    }
    
    public void OnSettingsClicked()
    {
        Console.WriteLine("Settings Clicked");
    }

    public void OnExitClicked()
    {
        _sceneManager.ApplicationCloseRequested = true;
    }
}