using System.Text;
using Cyberstar.Core;
using Cyberstar.Engine.Logging;
using Cyberstar.Extensions.IO;

namespace Cyberstar.ECS;

/// <summary>
/// Manages the creation and hierarchy of some entity pool.
/// </summary>
public class EntityManager
{
    /// <summary>
    /// The total number of entities in the manager.
    /// </summary>
    public uint Entities => (uint)(_entityCount - _destroyedEntities.Count);

    /// <summary>
    /// The total number of destroyed entities in the manager.
    /// </summary>
    public uint DestroyedEntities => (uint)_destroyedEntities.Count;
    
    /// <summary>
    /// The logger that the entity manager will use.
    /// </summary>
    private readonly ILogger _logger;
    
    /// <summary>
    /// The memory pool of all of the entities.
    /// </summary>
    private Memory<EntityWrapper> _allEntities;
    
    /// <summary>
    /// The queue of all of the entities that have been destroyed and can be reused.
    /// </summary>
    private Queue<int> _destroyedEntities;
    
    /// <summary>
    /// This is the maximum number of entities that existed at one time.
    /// </summary>
    private int _entityCount;

    /// <summary>
    /// The pointer to the last item in the relations memory.
    /// </summary>
    private int _relationTail;

    /// <summary>
    /// The memory pool of all of the entity relationships.
    /// </summary>
    private Memory<Relation> _entityRelations;
    
    /// <summary>
    /// The mapping from an entity to it's first child.
    /// </summary>
    private Dictionary<Entity, int> _entityToNodeIndex;
    
    /// <summary>
    /// The dictionary that maps types to component allocators.
    /// </summary>
    private Dictionary<Type, IComponentAllocator> _componentAllocators;

    /// <summary>
    /// The dictionary of global components that needed for the entity manager. This is things such as
    /// Cameras, frame timing etc...
    /// </summary>
    private Dictionary<Type, IComponent> _globalComponents;

    /// <summary>
    /// The collection of systems that will run for the manager.
    /// </summary>
    private List<ISystem> _systems;

    public EntityManager(ILogger logger, int initialCapacity)
    {
        _logger = logger;
        _allEntities = new Memory<EntityWrapper>(new EntityWrapper[initialCapacity]);
        _entityRelations = new Memory<Relation>(new Relation[initialCapacity]);
        _entityToNodeIndex = new Dictionary<Entity, int>();
        _destroyedEntities = new Queue<int>();
        _componentAllocators = new Dictionary<Type, IComponentAllocator>();
        _globalComponents = new Dictionary<Type, IComponent>();
        _systems = new List<ISystem>();
    }

    /// <summary>
    /// Adds the given system to the entity manager.
    /// </summary>
    /// <param name="system"></param>
    public void AddSystem(ISystem system)
    {
        _systems.Add(system);
        system.EntityManager = this;
    }

    /// <summary>
    /// Removes the system from the entity manager.
    /// </summary>
    /// <param name="system"></param>
    public void RemoveSystem(ISystem system)
    {
        _systems.Remove(system);
        system.EntityManager = null;
    }

    /// <summary>
    /// Executes all the systems in order.
    /// </summary>
    /// <param name="frameTiming"></param>
    public void RunSystems(FrameTiming frameTiming)
    {
        for (var i = 0; i < _systems.Count; i++)
            if (_systems[i].Enabled)
                _systems[i].PreUpdate();
        
        for (var i = 0; i < _systems.Count; i++)
            if (_systems[i].Enabled)
                _systems[i].Update(in frameTiming);
        
        for (var i = 0; i < _systems.Count; i++)
            if (_systems[i].Enabled)
                _systems[i].PostUpdate();
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

    /// <summary>
    /// Meant to retrieve all components that an entity has. This is not meant for ecs code.
    /// </summary>
    /// <param name="components"></param>
    /// <returns></returns>
    public bool TryGetAllComponents(Entity entity, List<IComponent> components)
    {
        var cnt = components.Count;
        foreach (var allocator in _componentAllocators.Values)
            if (allocator.HasComponentForEntity(entity))
                components.Add(allocator[entity]);

        return cnt < components.Count;
    }

    /// <summary>
    /// Attempts to get the component of the given type. If it does not exist, return false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool TryGetGlobalComponent<T>(out T t) where T : struct, IComponent
    {
        if (_globalComponents.TryGetValue(typeof(T), out var component))
        {
            t = (T)component;
            return true;
        }

        t = default;
        return false;
    }

    /// <summary>
    /// Sets the global component for the given type.
    /// </summary>
    /// <param name="t"></param>
    /// <typeparam name="T"></typeparam>
    public void SetGlobalComponent<T>(T t) where T : struct, IComponent
    {
        _globalComponents[typeof(T)] = t;
    }
    
    public T ComponentFor<T>(Entity entity) where T : struct, IComponent
    {
        var allocator = this.GetAllocatorFor<T>();
        return allocator.Data.Get(entity);
    }

    public ref T GetComponentFor<T>(Entity entity) where T : struct, IComponent
    {
        var allocator = this.GetAllocatorFor<T>();
        return ref allocator.Data.Get(entity);
    }

    public bool TryGetComponentFor<T>(Entity entity, out T outComponent) where T : struct, IComponent
    {
        var allocator = this.GetAllocatorFor<T>();
        if (allocator.Success)
            if (allocator.Data.TryGet(entity, out outComponent))
                return true;

        outComponent = default;
        return false;
    }
    
    public void SetComponentFor<T>(Entity entity, T component) where T : struct, IComponent
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
    
    public void RemoveComponentFor<T>(Entity entity) where T : struct, IComponent
    {
        var allocator = this.GetAllocatorFor<T>();
        if (allocator.Success)
            allocator.Data.Remove(entity);
    }

    public bool HasComponentFor<T>(Entity entity) where T : struct, IComponent
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

    public uint FindEntitiesWith(ReadOnlySpan<Type> allocatorTypes, Span<Entity> entityBuffer)
    {
        var cnt = 0u;

        for (var i = 0; i < _entityCount && cnt < entityBuffer.Length; i++)
        {
            var hasAllAllocators = true;
            for (var j = 0; j < allocatorTypes.Length; j++)
            {
                var allocator = _componentAllocators[allocatorTypes[j]];
                var entityWrapper = _allEntities.Span[(int)i];
                if (!entityWrapper.Alive || !allocator.HasComponentForEntity(entityWrapper.Entity))
                {
                    hasAllAllocators = false;
                    break;
                }                
            }
        
            if (hasAllAllocators)
            {
                var entityWrapper = _allEntities.Span[i];
                entityBuffer[(int)cnt] = entityWrapper.Entity;
                cnt++;
            }
        }

        return cnt;
    }

    /*
    public IEnumerable<Entity> FindEntities(ReadOnlySpan<IntPtr> allocatorTypes)
    {
        for (var i = 0; i < _entityCount; i++)
        {
            var hasAllAllocators = true;
            for (var j = 0; j < allocatorTypes.Length; j++)
            {
                var type = RuntimeTypeHandle.FromIntPtr(allocatorTypes[j]).GetType();
                var allocator = _componentAllocators[type];
                var entityWrapper = _allEntities.Span[i];
                if (!entityWrapper.Alive || !allocator.HasComponentForEntity(entityWrapper.Entity))
                {
                    hasAllAllocators = false;
                    break;
                }                
            }
        
            if (hasAllAllocators)
            {
                var entityWrapper = _allEntities.Span[i];
                yield return entityWrapper.Entity;
            }
        }
    }
    */
    
    public (uint foundEntities, uint scanEnd) FindEntitiesChunked(ReadOnlySpan<Type> allocatorTypes, Span<Entity> entityBuffer, uint offset = 0)
    {
        var cnt = 0u;
        var i = (int)offset;

        for (; i < _entityCount && cnt < entityBuffer.Length; i++)
        {
            var hasAllAllocators = true;
            for (var j = 0; j < allocatorTypes.Length; j++)
            {
                var allocator = _componentAllocators[allocatorTypes[j]];
                var entityWrapper = _allEntities.Span[i];
                if (!entityWrapper.Alive || !allocator.HasComponentForEntity(entityWrapper.Entity))
                {
                    hasAllAllocators = false;
                    break;
                }                
            }
        
            if (hasAllAllocators)
            {
                var entityWrapper = _allEntities.Span[i];
                entityBuffer[(int)cnt] = entityWrapper.Entity;
                cnt++;
            }
        }

        return (cnt, (uint)i);
    }

    public void Serialize(Stream stream)
    {
        using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
        {
            // Serialize the entities
            writer.Write(_entityCount);
            writer.Write(_relationTail);
            
            writer.Write(_allEntities.Length);
            for (var i = 0; i < _entityCount; i++)
            {
                var wrapper = _allEntities.Span[i];
                writer.Write(wrapper.Entity);
                writer.Write(wrapper.Alive);
            }
            
            // Serialize the destroyed entities
            writer.Write(_destroyedEntities.Count);
            foreach (var thing in _destroyedEntities)
            {
                writer.Write(thing);
            }

            // Write the relations
            writer.Write(_entityRelations.Length);
            for (var i = 0; i < _entityRelations.Length; i++)
            {
                var relation = _entityRelations.Span[i];
                writer.Write(relation.Child);
                writer.Write(relation.Next);
                writer.Write(relation.Previous);
            }
            
            // Write the Relation mapping
            writer.Write(_entityToNodeIndex.Count);
            foreach (var keyPair in _entityToNodeIndex)
            {
                var index = keyPair.Value;
                writer.Write(keyPair.Key);
                writer.Write(index);
            }
            
            // Write the allocator data
            writer.Write(_componentAllocators.Count);
            foreach (var keyValue in _componentAllocators)
            {
                // Write the component key name
                writer.Write(keyValue.Key.FullName);
                // Write the allocator class name
                var allocatorName = keyValue.Value.GetType().FullName;
                writer.Write(allocatorName);
                keyValue.Value.Serialize(writer);
            }
        }
    }

    public void Deserialize(Stream stream)
    {
        using (var reader = new BinaryReader(stream))
        {
            // Deserialize the entities
            _entityCount = reader.ReadInt32();
            _relationTail = reader.ReadInt32();

            var allEntitiesLength = reader.ReadInt32();
            _allEntities = new Memory<EntityWrapper>(new EntityWrapper[allEntitiesLength]);
            for (var i = 0; i < _entityCount; i++)
            {
                var wrapper = new EntityWrapper(reader.ReadEntity(), reader.ReadBoolean());
                _allEntities.Span[i] = wrapper;
            }
            
            // Deserialize the destroyed entities
            var destroyedEntitiesLength = reader.ReadInt32();
            _destroyedEntities = new Queue<int>(destroyedEntitiesLength);
            for (var i = 0; i < destroyedEntitiesLength; i++)
            {
                _destroyedEntities.Enqueue(reader.ReadInt32());
            }
            
            // Read the relations
            var relationsLength = reader.ReadInt32();
            _entityRelations = new Memory<Relation>(new Relation[relationsLength]);
            for (var i = 0; i < relationsLength; i++)
            {
                var relation = new Relation(reader.ReadEntity(), reader.ReadInt32(), reader.ReadInt32());
                _entityRelations.Span[i] = relation;
            }
            
            // Read the relation mapping
            var entityToNodeCount = reader.ReadInt32();
            _entityToNodeIndex = new Dictionary<Entity, int>();
            for (var i = 0; i < entityToNodeCount; i++)
            {
                var entity = reader.ReadEntity();
                _entityToNodeIndex[entity] = reader.ReadInt32();
            }
            
            // Read the allocator data
            var allocators = reader.ReadInt32();
            _componentAllocators = new Dictionary<Type, IComponentAllocator>();
            for (var i = 0; i < allocators; i++)
            {
                var componentName = reader.ReadString();
                var componentType = Type.GetType(componentName);
                
                var allocatorName = reader.ReadString();
                var type = Type.GetType(allocatorName);
                var allocator = (IComponentAllocator)Activator.CreateInstance(type);
                allocator.Deserialize(reader);

                _componentAllocators[componentType] = allocator; 
            }
        }
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