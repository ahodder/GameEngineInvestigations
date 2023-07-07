using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.Logging;
using Cyberstar.UI;
using Raylib_cs;

namespace Cyberstar.Scenes;

public class MainMenuScene : Scene
{
    private SceneManager _sceneManager;
    private UiRenderer _uiRenderer;

    public MainMenuScene(SceneManager sceneManager, ILogger logger, AssetManager assets) : base(logger, assets)
    {
        var layout = new VerticalLayoutView(assets)
            .AddView(CreateButton("Continue", OnContinueClicked))
            .AddView(CreateButton("New Game", OnNewGameClicked))
            .AddView(CreateButton("Load Game", OnLoadGameClicked))
            .AddView(CreateButton("Settings", OnSettingsClicked))
            .AddView(CreateButton("Exit", OnExitClicked));

        _sceneManager = sceneManager;
        _uiRenderer = new UiRenderer(layout, 0, 0, 500, 500);
    }
    
    public override void PerformTick(FrameTiming frameTiming)
    {
        RenderUi();
    }

    
    public void RenderUi() 
    {
        _uiRenderer.Render();
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

    private ButtonView CreateButton(string text, Action handler)
    {
        return new ButtonView(Assets)
        {
            Text = text,
            FontSize = 24, 
            BackgroundColor = Color.GRAY,
            Padding = new Thickness().Set(5),
            Margin = new Thickness().Set(10),
            OnClick = handler,
        };
    }
}