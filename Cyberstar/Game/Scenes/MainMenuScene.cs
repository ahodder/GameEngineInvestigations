using Cyberstar.Core;
using Cyberstar.Engine;
using Cyberstar.Engine.Scenes;
using Cyberstar.UI;
using Raylib_cs;

namespace Cyberstar.Game.Scenes;

public class MainMenuScene : Scene
{
    private UiRenderer _uiRenderer;

    public MainMenuScene(IEngine engine) : base(engine)
    {
        var layout = new VerticalLayoutView(engine.AssetManager);
        layout.AddView(CreateButton("Continue", OnContinueClicked));
        layout.AddView(CreateButton("New Game", OnNewGameClicked));
        layout.AddView(CreateButton("Load Game", OnLoadGameClicked));
        layout.AddView(CreateButton("Settings", OnSettingsClicked));
        layout.AddView(CreateButton("Exit", OnExitClicked));
        layout.AddView(CreateButton("Entity Editor", OnEntityEditorClicked));

        _uiRenderer = new UiRenderer(layout, 0, 0, 500, 500);
        
        // engine.AudioManager.PlayMusic("cyberpunk", true);
    }
    
    public override void PerformTick(FrameTiming frameTiming)
    {
        RenderUi(in frameTiming);
    }

    public override void OnSceneUnloaded()
    {
        Engine.AudioManager.StopMusic();
    }

    public void RenderUi(in FrameTiming frameTiming) 
    {
        _uiRenderer.Render(in frameTiming);
    }

    public void OnContinueClicked()
    {
        Console.WriteLine("Continue Clicked");
    }
    
    public void OnNewGameClicked()
    {
        Engine.SceneManager.BeginLoadActiveScene(new TestoScene(Engine));
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
        Engine.SceneManager.ApplicationCloseRequested = true;
    }

    public void OnEntityEditorClicked()
    {
        Engine.SceneManager.BeginLoadActiveScene(new ShipBuilderScene(Engine));
    }

    private ButtonView CreateButton(string text, Action handler)
    {
        return new ButtonView(Engine.AssetManager)
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