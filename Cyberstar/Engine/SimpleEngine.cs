using Cyberstar.Core;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.Engine.Logging;
using Cyberstar.Engine.Scenes;
using Cyberstar.Engine.Sounds;

namespace Cyberstar.Engine;

public class SimpleEngine : IEngine
{
    public ILogger Logger { get; }
    public AssetManager AssetManager { get; }
    public AudioManager AudioManager { get; }
    public SceneManager SceneManager { get; }
    public WindowData WindowData { get; }

    public SimpleEngine(ILogger logger, AssetManager assetManager, WindowData windowData)
    {
        Logger = logger;
        AssetManager = assetManager;
        AudioManager = new AudioManager(assetManager);
        SceneManager = new SceneManager(this);
        WindowData = windowData;
    }
}