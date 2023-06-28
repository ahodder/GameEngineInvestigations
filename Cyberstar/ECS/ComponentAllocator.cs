using System.Runtime.CompilerServices;
using Cyberstar.Core;

namespace Cyberstar.ECS;

public interface IComponentAllocator
{
    /// <summary>
    /// Whether or not the component allocator has a component for the given entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool HasComponentForEntity(Entity entity);

    /// <summary>
    /// Serializes the component allocator to the given stream.
    /// </summary>
    /// <param name="writer"></param>
    /// <returns></returns>
    Result<bool> SerializeTo(BinaryWriter writer);
    
    /// <summary>
    /// Deserializes content from the stream into the allocator.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    Result<bool> DeserializeFrom(BinaryReader reader);
}

public interface IComponentAllocator<T> : IComponentAllocator where T : struct
{
    /// <summary>
    /// Attempts to get the given component type for the provided entity. 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    bool TryGet(Entity entity, out T component);
    
    /// <summary>
    /// Adds or updates the component for the given entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="component"></param>
    void Set(Entity entity, T component);

    /// <summary>
    /// Removes the component association for the given entity. 
    /// </summary>
    /// <param name="entity"></param>
    void Remove(Entity entity);
}

public class ComponentAllocator<T> : IComponentAllocator<T> where T : struct
{
    private const int InitialCapacity = 16;
    
    private readonly Dictionary<Entity, int> _entityToIndex;
    private readonly Dictionary<int, Entity> _indexToEntity;
    private Memory<T> _components;
    private int _head;

    public ComponentAllocator() : this(InitialCapacity)
    {
    }

    public ComponentAllocator(int initialCapacity = InitialCapacity)
    {
        _entityToIndex = new Dictionary<Entity, int>();
        _indexToEntity = new Dictionary<int, Entity>();
        _components = new Memory<T>(new T[initialCapacity]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(Entity entity, T component)
    {
        _entityToIndex[entity] = _head;
        _indexToEntity[_head] = entity;
        _components.Span[_head] = component;
        _head++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(Entity entity, out T outValue) {
        if (_entityToIndex.TryGetValue(entity, out var index))
        {
            outValue = _components.Span[index];
            return true;
        }

        outValue = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(Entity entity, T component)
    {
        if (_entityToIndex.TryGetValue(entity, out var index))
            _components.Span[index] = component;
        else
        {
            index = _head++;
            _components.Span[index] = component;
            _entityToIndex[entity] = index;
            _indexToEntity[index] = entity;
        }
    }

    public void Remove(Entity entity)
    {
        if (_entityToIndex.TryGetValue(entity, out var index))
        {
            _head--;
            // Update the moved entity
            var movedEntity = _indexToEntity[_head];
            _entityToIndex[movedEntity] = index;
            _indexToEntity[index] = movedEntity;
            _components.Span[index] = _components.Span[_head];
            
            // Remove the entity
            _entityToIndex.Remove(entity);
            _indexToEntity.Remove(index);
        }
    }

    public bool HasComponentForEntity(Entity entity)
    {
        return _entityToIndex.ContainsKey(entity);
    }

    /// <summary>
    /// Doubles the current size of the component allocator
    /// </summary>
    public void Expand()
    {
        var tmp = new Memory<T>(new T[_components.Length * 2]);
        _components.CopyTo(tmp);
        _components = tmp;
    }

    public Result<bool> SerializeTo(BinaryWriter writer)
    {
        try
        {
            ReadOnlySpan<byte> bytes = Unsafe.As<Memory<T>, Memory<byte>>(ref _components).Span;
            writer.Write(bytes.Length);
            writer.Write(bytes);
            writer.Write(_head);
            foreach (var pair in _entityToIndex)
            {
                // Write the entity
                writer.Write(pair.Key.Id);
                writer.Write(pair.Key.Generation);
                // Write the index
                writer.Write(pair.Value);
            }
            return Result<bool>.DefaultSuccess;
        }
        catch (Exception e)
        {
            return new Result<bool>($"Failed to write allocations for {typeof(T)} to stream", e);
        }
    }

    public Result<bool> DeserializeFrom(BinaryReader reader)
    {
        try
        {
            var byteLength = reader.ReadInt32();
            var bytes = reader.ReadBytes(byteLength);
            var rawBytes = new Memory<byte>(bytes);
            _components = Unsafe.As<Memory<byte>, Memory<T>>(ref rawBytes);
            _head = reader.ReadInt32();
            for (var i = 0; i < _head; i++)
            {
                var id = reader.ReadInt32();
                var generation = reader.ReadInt32();
                var index = reader.ReadInt32();
                var entity = new Entity(id, generation);
                _indexToEntity[index] = entity;
                _entityToIndex[entity] = index;
            }

            return Result<bool>.DefaultSuccess;
        }
        catch (Exception e)
        {
            return new Result<bool>($"Failed to read allocations for {typeof(T)} from stream", e);
        }
    }
}