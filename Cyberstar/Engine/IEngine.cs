using Cyberstar.Core;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.Engine.Logging;
using Cyberstar.Engine.Scenes;
using Cyberstar.Engine.Sounds;

namespace Cyberstar.Engine;

public interface IEngine
{
    ILogger Logger { get; }
    AssetManager AssetManager { get; }
    AudioManager AudioManager { get; }
    SceneManager SceneManager { get; }
    
    WindowData WindowData { get; }
}