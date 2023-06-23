using System.Collections;

namespace Cyberstar.ECS;

public readonly ref struct ComponentQuery<T> where T : struct
{
    public readonly Span<Entity> Entities;
    public readonly Span<T> Components;

    public ComponentQuery(Span<Entity> entities, Span<T> components)
    {
        Entities = entities;
        Components = components;
    }
}

public readonly ref struct ComponentQuery<T1, T2>
    where T1 : struct
    where T2 : struct
{
    public readonly Span<Entity> Entities;
    public readonly Span<(T1, T2)> Components;
    
    public ComponentQuery(Span<Entity> entities, Span<(T1, T2)> components)
    {
        Entities = entities;
        Components = components;
    }
}

public readonly ref struct ComponentQuery<T1, T2, T3>
    where T1 : struct
    where T2 : struct
    where T3 : struct 
{
    public readonly Span<Entity> Entities;
    public readonly Span<(T1, T2, T3)> Components;
    
    public ComponentQuery(Span<Entity> entities, Span<(T1, T2, T3)> components)
    {
        Entities = entities;
        Components = components;
    }
}