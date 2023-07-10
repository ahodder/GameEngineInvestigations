using Cyberstar.Core;

namespace Cyberstar.ECS;

public abstract class SystemBase : ISystem
{
    public bool Enabled { get; set; } = true;

    public EntityManager? EntityManager { get; set; }

    public virtual void PreUpdate()
    {
    }

    public abstract void Update(in FrameTiming frameTiming);

    public virtual void PostUpdate()
    {
    }
}