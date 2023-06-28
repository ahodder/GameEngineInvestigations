using Cyberstar.Core;
using Cyberstar.Logging;

namespace Cyberstar.ECS;

/// <summary>
/// Manages the creation and hierarchy of some entity pool.
/// </summary>
public class EntityManager
{
    /// <summary>
    /// The total number of entities in the manager.
    /// </summary>
    public int Entities => _entityCount - _destroyedEntities.Count;

    /// <summary>
    /// The total number of destroyed entities in the manager.
    /// </summary>
    public int DestroyedEntities => _destroyedEntities.Count;
    
    private ILogger _logger;
    private Memory<EntityWrapper> _allEntities;
    private Memory<Relation> _entityRelations;
    
    /// <summary>
    /// The mapping from an entity to it's first child.
    /// </summary>
    private Dictionary<Entity, int> _entityToNodeIndex;
    private Queue<int> _destroyedEntities;
    
    /// <summary>
    /// The dictionary that maps types to component allocators.
    /// </summary>
    private Dictionary<Type, IComponentAllocator> _componentAllocators;

    /// <summary>
    /// This is the maximum number of entities that existed at one time.
    /// </summary>
    private int _entityCount;

    /// <summary>
    /// The pointer to the last item in the relations memory.
    /// </summary>
    private int _relationTail;

    public EntityManager(ILogger logger, int initialCapacity)
    {
        _logger = logger;
        _allEntities = new Memory<EntityWrapper>(new EntityWrapper[initialCapacity]);
        _entityRelations = new Memory<Relation>(new Relation[initialCapacity]);
        _entityToNodeIndex = new Dictionary<Entity, int>();
        _destroyedEntities = new Queue<int>();
        _componentAllocators = new Dictionary<Type, IComponentAllocator>();
    }

    /// <summary>
    /// Queries whether or not the given entity exists.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool DoesEntityExist(Entity entity)
    {
        if (entity.Id >= _entityCount || entity.Id < 0)
            return false;

        var wrapper = _allEntities.Span[entity.Id];
        return wrapper.Alive && wrapper.Entity.Generation == entity.Generation;
    }

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <returns></returns>
    public Entity CreateEntity()
    {
        if (_entityCount == _allEntities.Length)
            Resize();

        if (_destroyedEntities.Count > 0)
        {
            var cachedRet = _destroyedEntities.Dequeue();
            var wrapper = _allEntities.Span[cachedRet];
            wrapper = new EntityWrapper(wrapper.Entity.NewGeneration(), true);
            _allEntities.Span[cachedRet] = wrapper;
            return wrapper.Entity;
        }

        var newEntity = new EntityWrapper(new Entity(_entityCount), true);
        _allEntities.Span[_entityCount] = newEntity;
        _entityCount++;
        return newEntity.Entity;
    }

    /// <summary>
    /// Destroys the given entity and all of its children.
    /// </summary>
    /// <param name="entity"></param>
    public void DestroyEntity(Entity entity)
    {
        _allEntities.Span[entity.Id] = new EntityWrapper(entity, false);
        _destroyedEntities.Enqueue(entity.Id);
    }

    public bool SetParentFor(Entity parent, Entity child)
    {
        if (!DoesEntityExist(child) || !DoesEntityExist(parent))
        {
#if DEBUG
            _logger.Error("Attempted to set parent for entity that doesn't exist");
#endif
            return false;
        }

        if (_relationTail >= _entityRelations.Length)
            ResizeRelations();
        
        if (_entityToNodeIndex.TryGetValue(parent, out var firstChildIndex))
        {
            // Set the next of the previous to the next item
            // Set the previous of the first item to the new item
            // Set the previous of the new item to the old previous
            // Set the next of the new item to the first item
            ref var firstChild = ref _entityRelations.Span[firstChildIndex];
            ref var previous = ref _entityRelations.Span[firstChild.Previous];

            var newItem = new Relation(child, firstChild.Previous, firstChildIndex);
            previous.Next = _relationTail;
            firstChild.Previous = _relationTail;
            _entityRelations.Span[_relationTail++] = newItem;
        }
        else
        {
            // This is a new Relation insertion, so we need to set the pointers to ourselves
            var relations = new Relation(child, _relationTail, _relationTail);
            _entityRelations.Span[_relationTail] = relations;
            _entityToNodeIndex[parent] = _relationTail;
            _relationTail++;
        }
        
        return true;
    }

    public bool RemoveChildFor(Entity parent, Entity child)
    {
        if (!DoesEntityExist(child) || !DoesEntityExist(parent))
        {
#if DEBUG
            _logger.Error("Attempted to remote parent for entity that doesn't exist");
#endif
            return false;
        }

        if (!_entityToNodeIndex.TryGetValue(parent, out var firstChildIndex)) return false;
        
        var firstChild = _entityRelations.Span[firstChildIndex];
        
        // Are we deleting the first child? If so, we need to update our index.
        if (child == firstChild.Child)
        {
            _entityToNodeIndex.Remove(child);
            _entityToNodeIndex[parent] = firstChild.Next;
        }
        else
        {
            while (firstChild.Child != child) firstChild = _entityRelations.Span[firstChild.Next];
        }
        
        // If the child isn't the only child, update the next and previous
        ref var previous = ref _entityRelations.Span[firstChild.Previous];
        ref var next = ref _entityRelations.Span[firstChild.Next];
        
        var isOnlyChild = firstChild.Next == firstChildIndex && firstChild.Previous == firstChildIndex;
        if (!isOnlyChild)
        {
            previous.Next = firstChild.Next;
            next.Previous = firstChild.Previous;
        }
        else
        {
            // Remove the child relationship.
            _entityToNodeIndex.Remove(parent);
            // Move the tail to the empty slot
            var tail = _entityRelations.Span[_relationTail];
            previous = ref _entityRelations.Span[tail.Previous];
            next = ref _entityRelations.Span[tail.Next];
            // And update the relationships to the new location.
            previous.Next = firstChildIndex;
            next.Previous = firstChildIndex;
        }

        _relationTail--;
        return true;
    }

    public bool IsChildOf(Entity parent, Entity child)
    {
        if (!DoesEntityExist(child) || !DoesEntityExist(parent))
        {
#if DEBUG
            _logger.Error("Attempted to determine an entity's parent for entity that doesn't exist");
#endif
            return false;
        }

        if (_entityToNodeIndex.TryGetValue(parent, out var firstChildIndex))
        {
            var currentChild = _entityRelations.Span[firstChildIndex];
            var firstChild = currentChild;

            do
            {
                if (currentChild.Child == child)
                {
                    return true;
                }
                
                currentChild = _entityRelations.Span[currentChild.Next];
            } while (currentChild.Child != firstChild.Child);
        }

        return false;
    }

    public int GetChildrenCount(Entity parentEntity)
    {
        if (_entityToNodeIndex.TryGetValue(parentEntity, out var index))
        {
            var count = 1;
            var relation = _entityRelations.Span[index];
            while (relation.Next != index)
            {
                relation = _entityRelations.Span[relation.Next];
                count++;
            }

            return count;
        }
        else
        {
            return 0;
        }
    }
    
    /// <summary>
    /// Resizes the entity pool to twice the existing size.
    /// </summary>
    public void Resize()
    {
        var newMem = new Memory<EntityWrapper>(new EntityWrapper[_allEntities.Length * 2]);
        _allEntities.CopyTo(newMem);
        _allEntities = newMem;
    }

    /// <summary>
    /// Resizes the relation pool to twice the existing size.
    /// </summary>
    public void ResizeRelations()
    {
        var newMem = new Memory<Relation>(new Relation[_entityRelations.Length]);
        _entityRelations.CopyTo(newMem);
        _entityRelations = newMem;
    }

    public bool TryGetComponentFor<T>(Entity entity, out T outComponent) where T : struct
    {
        var allocator = this.GetAllocatorFor<T>();
        if (allocator.Success)
            if (allocator.Data.TryGet(entity, out outComponent))
                return true;

        outComponent = default;
        return false;
    }
    
    public void SetComponentFor<T>(Entity entity, T component) where T : struct
    {
        var allocator = this.GetAllocatorFor<T>();
        if (allocator.Success)
        {
            if (allocator.Data.HasComponentForEntity(entity))
                allocator.Data.Set(entity, component);
            else
                allocator.Data.Add(entity, component);
        }
    }
    
    public void RemoveComponentFor<T>(Entity entity) where T : struct
    {
        var allocator = this.GetAllocatorFor<T>();
        if (allocator.Success)
            allocator.Data.Remove(entity);
    }

    public bool HasComponentFor<T>(Entity entity) where T : struct
    {
        var allocator = this.GetAllocatorFor<T>();
        if (allocator.Success)
            return allocator.Data.HasComponentForEntity(entity);
        return false;
    } 

    public Result<IComponentAllocator> GetAllocatorFor(Type type)
    {
        if (_componentAllocators.TryGetValue(type, out var allocator))
            return allocator.Success();

        return new Result<IComponentAllocator>($"Failed to find allocator for {type.Name}");
    }

    public void SetAllocatorFor(Type type, IComponentAllocator allocator)
    {
        _componentAllocators[type] = allocator;
    }
    
    public uint FindEntitiesWith(Type[] allocatorTypes, Entity[] entityBuffer, uint offset = 0, uint count = int.MaxValue)
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        var allocators = new List<IComponentAllocator>();
        
        // Ensure all allocators are present
        for (var i = 0; i < allocatorTypes.Length; i++)
        {
            if (_componentAllocators.TryGetValue(allocatorTypes[i], out var a))
                allocators.Add(a);
            else
                return 0;
        }
        
        var max = Math.Min(entityBuffer.Length, offset + count);
        var cnt = 0u;

        for (var i = offset; i < max; i++)
        {
            var hasAllAllocators = true;
            for (var j = 0; j < allocators.Count; j++)
            {
                var allocator = allocators[j];
                var entityWrapper = _allEntities.Span[(int)i];
                if (!entityWrapper.Alive || !allocator.HasComponentForEntity(entityWrapper.Entity))
                {
                    hasAllAllocators = false;
                    break;
                }                
            }
        
            if (hasAllAllocators)
            {
                var k = offset + cnt;
                var entityWrapper = _allEntities.Span[(int)k];
                entityBuffer[cnt] = entityWrapper.Entity;
                cnt++;
            }
        }

        return cnt;
    }

    /// <summary>
    /// Represents whether or not an entity is alive.
    /// </summary>
    private readonly struct EntityWrapper
    {
        public readonly Entity Entity;
        public readonly bool Alive;

        public EntityWrapper(Entity entity, bool alive)
        {
            Entity = entity;
            Alive = alive;
        }
    }

    /// <summary>
    /// Represents the doubly-linked list of all of the children of some entity.
    /// </summary>
    private struct Relation
    {
        public Entity Child;
        public int Previous;
        public int Next;

        public Relation(Entity child, int previous, int next)
        {
            Child = child;
            Previous = previous;
            Next = next;
        }

        public override string ToString() => $"Child: {Child}, Previous: {Previous}, Next: {Next}";
    }
}