using Cyberstar.Core;

namespace Cyberstar.ECS;

public abstract class ComponentIteratorSystemBase : SystemBase
{
    private readonly Type[] _allocatorTypes;
    
    public ComponentIteratorSystemBase(Type[] allocatorTypes)
    {
        _allocatorTypes = allocatorTypes;
    }
    
    public sealed override void Update(in FrameTiming deltaTime)
    {
        if (EntityManager == null) return;
        
        Span<Entity> entityBuffer = stackalloc Entity[128];
        var entities = EntityManager.Entities;
        var offset = 0u;
        do
        {
            var result = EntityManager.FindEntitiesChunked(_allocatorTypes.AsSpan(), entityBuffer, offset);
            HandleEntities(in deltaTime, entityBuffer, result.foundEntities);
            offset = result.scanEnd;
        } while (offset < entities);
    }

    protected abstract void HandleEntities(in FrameTiming frameTiming, ReadOnlySpan<Entity> entities, uint count);
}