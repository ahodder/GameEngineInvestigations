using Cyberstar.AssetManagement;
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

    public Scene(ILogger logger, AssetManager assets)
    {
        Assets = assets;
        EntityManager = new EntityManager(logger, 128);
    }
    
    public abstract void ReadInput();
    public abstract void UpdateState();
    public abstract void Render();
}