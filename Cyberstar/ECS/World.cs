using Cyberstar.Core;

namespace Cyberstar.ECS;

public class World
{
    #region Properties

    public int Entities => _entityCount - _destroyedEntityCount;
    public int DestroyedEntities => _destroyedEntityCount;
    
    #endregion Properties
    
    #region Fields
    
    private Memory<Entity> _entities;
    private Memory<Relation> _relationships;
    private Dictionary<Entity, Relation> _parentNodes;

    private int[] _destroyedEntities;
    private int _entityCount;
    private int _destroyedEntityCount;
    private Dictionary<Type, IComponentAllocator> _allocators;
    
    private List<ISystem> _systems;
    
    #endregion Fields

    public World(int initialCapacity = 16)
    {
        _allocators = new Dictionary<Type, IComponentAllocator>();
        
        _entities = new Entity[initialCapacity];
        _destroyedEntities = new int[initialCapacity];
        _entityCount = 0;
        _destroyedEntityCount = 0;

        _systems = new List<ISystem>();
    }

    public World RegisterSystem(ISystem system)
    {
        _systems.Add(system);
        return this;
    }
    
    public void AdvanceWorldSimulation(FrameTiming time)
    {
        for (var i = 0; i < _systems.Count; i++)
        {
            var service = _systems[i];
            if (!service.Enabled) continue;
            
            service.PreUpdate();
            service.Update(time);
            service.PostUpdate();
        }
    }

    public Entity CreateEntity()
    {
        if (_destroyedEntityCount > 0)
        {
            var i = --_destroyedEntityCount;
            var entity = _destroyedEntities[i];
            return _entities.Span[entity];
        }

        var index = _entityCount++;
        var ret = new Entity(index, _entities.Span[index].Generation);
        _entities.Span[index] = ret;
        return ret;
    }

    public bool IsDestroyed(Entity entity)
    {
        var entityInBounds = entity.Id < _entityCount;
        var generationIsCurrent = entity.Generation == _entities.Span[entity.Id].Generation;
        return !entityInBounds || !generationIsCurrent;
    }

    public void DestroyEntity(Entity entity)
    {
        entity = entity.NewGeneration();
        _entities.Span[entity.Id] = entity;
        _destroyedEntities[_destroyedEntityCount++] = entity.Id;
    }
    
    public void SetComponentFor<T>(Entity entity, T component) where T : struct
    {
        // var allocator = this.GetAllocatorFor<T>();
        // if (allocator.Success)
        // {
        //     if (allocator.Data.HasComponentForEntity(entity))
        //     {
        //         allocator.Data.Set(entity, component);
        //     }
        //     else
        //     {
        //         allocator.Data.Add(entity, component);
        //     } 
        // }
    }

    public void RemoveComponentFor<T>(Entity entity) where T : struct
    {
        // var allocator = this.GetAllocatorFor<T>();
        // if (allocator.Success)
        // {
        //     allocator.Data.Remove(entity);
        // }
    }

    public bool TryGetAllocatorFor(Type type, out IComponentAllocator allocator)
    {
        if (_allocators.TryGetValue(type, out allocator))
            return true;

        allocator = default;
        return false;
    }

    public void SetParentFor(Entity parent, Entity child)
    {
        
    }

    public void SetAllocatorFor(Type type, IComponentAllocator allocator)
    {
        _allocators[type] = allocator;
    }

    public int FindEntitiesWith(Type[] allocatorTypes, Entity[] entityBuffer, int offset = 0, int count = int.MaxValue)
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        var allocators = new List<IComponentAllocator>();
        
        // Ensure all allocators are present
        for (var i = 0; i < allocatorTypes.Length; i++)
        {
            if (_allocators.TryGetValue(allocatorTypes[i], out var a))
            {
                allocators.Add(a);
            }
            else
            {
                return 0;
            }
        }
        
        var max = Math.Min(entityBuffer.Length, offset + count);
        var cnt = 0;

        for (var i = offset; i < max; i++)
        {
            var hasAllAllocators = true;
            for (var j = 0; j < allocators.Count; j++)
            {
                var allocator = allocators[j];
                if (!allocator.HasComponentForEntity(_entities.Span[i]))
                {
                    hasAllAllocators = false;
                    break;
                }                
            }

            if (hasAllAllocators)
            {
                var k = offset + cnt;
                entityBuffer[cnt] = _entities.Span[k];
                cnt++;
            }
        }

        return cnt;
    }
    
    private struct Relation
    {
        public readonly Entity Parent;
        public readonly Entity Child;
        /// <summary>
        /// The index in the relation buffer pointing to the next child of the parent.
        /// </summary>
        public readonly int NextChild;

        /// <summary>
        /// The index in the relation buffer pointing to the previous child of the parent.
        /// </summary>
        public readonly int PreviousChild;

        public Relation(Entity parent, Entity child)
        {
            Parent = parent;
            Child = child;
        }
    }
}
