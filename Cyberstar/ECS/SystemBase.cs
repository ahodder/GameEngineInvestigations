using Cyberstar.Core;

namespace Cyberstar.ECS;

public abstract class SystemBase : ISystem
{
    public bool Enabled { get; set; } = true;
    
    public virtual void PreUpdate()
    {
    }

    public abstract void Update(FrameTiming deltaTime);

    public virtual void PostUpdate()
    {
    }
}