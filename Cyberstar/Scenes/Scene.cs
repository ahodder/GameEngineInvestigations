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
    public AssetManager Assets { get; }
    public EntityManager EntityManager { get; }
    
    protected ILogger _logger { get; }

    public Scene(ILogger logger, AssetManager assets)
    {
        Assets = assets;
        EntityManager = new EntityManager(logger, 128);

        _logger = logger;
    }

    public abstract void PerformTick(FrameTiming frameTiming);
}