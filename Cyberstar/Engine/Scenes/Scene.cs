using Cyberstar.Core;

namespace Cyberstar.Engine.Scenes;

/// <summary>
/// Represents an encapsulation of a game state. 
/// </summary>
public abstract class Scene
{
    public IEngine Engine { get; }
    
    public Scene(IEngine engine)
    {
        Engine = engine;
    }

    public virtual void OnSceneLoaded()
    {
    }

    public abstract void PerformTick(FrameTiming frameTiming);

    public virtual void OnSceneUnloaded()
    {
    }
}