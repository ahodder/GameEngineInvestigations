using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Logging;

namespace Cyberstar.Scenes;

/// <summary>
/// Represents an encapsulation of a game state. 
/// </summary>
public abstract class Scene
{
    public WindowData WindowData { get; }
    public AssetManager Assets { get; }
    public EntityManager EntityManager { get; }
    
    protected ILogger _logger { get; }

    public Scene(ILogger logger, WindowData windowData, AssetManager assets)
    {
        WindowData = windowData;
        Assets = assets;
        EntityManager = new EntityManager(logger, 128);

        _logger = logger;
    }

    public abstract void PerformTick(FrameTiming frameTiming);
}