using Cyberstar.Core;

namespace Cyberstar.ECS;

public static class EntityManagerExtensions
{
    public static Result<ComponentAllocator<T>> GetAllocatorFor<T>(this EntityManager entityManager) where T : struct, IComponent
    {
        var result = entityManager.GetAllocatorFor(typeof(T));
        if (!result.Success)
        {
            var ret = Activator.CreateInstance<ComponentAllocator<T>>();
            entityManager.SetAllocatorFor(typeof(T), ret);
            return ret.Success();
        }
        return new Result<ComponentAllocator<T>>((ComponentAllocator<T>)result.Data);
    }

    public static void SetComponentsFor<T1, T2>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2)
        where T1 : struct, IComponent 
        where T2 : struct, IComponent
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
    }
    
    public static void SetComponentsFor<T1, T2, T3>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2, T3 t3)
        where T1 : struct, IComponent 
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
        entityManager.SetComponentFor(entity, t3);
    }
    
    public static void SetComponentsFor<T1, T2, T3, T4>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2, T3 t3, T4 t4)
        where T1 : struct, IComponent 
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
        entityManager.SetComponentFor(entity, t3);
        entityManager.SetComponentFor(entity, t4);
    }
    
    public static void SetComponentsFor<T1, T2, T3, T4, T5>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        where T1 : struct, IComponent 
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
        entityManager.SetComponentFor(entity, t3);
        entityManager.SetComponentFor(entity, t4);
        entityManager.SetComponentFor(entity, t5);
    }
    
    public static void SetComponentsFor<T1, T2, T3, T4, T5, T6>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        where T1 : struct, IComponent 
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
        entityManager.SetComponentFor(entity, t3);
        entityManager.SetComponentFor(entity, t4);
        entityManager.SetComponentFor(entity, t5);
        entityManager.SetComponentFor(entity, t6);
    }
    
    
    public static void SetComponentsFor<T1, T2, T3, T4, T5, T6, T7>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
        entityManager.SetComponentFor(entity, t3);
        entityManager.SetComponentFor(entity, t4);
        entityManager.SetComponentFor(entity, t5);
        entityManager.SetComponentFor(entity, t6);
        entityManager.SetComponentFor(entity, t7);
    }
    
    public static uint FindEntitiesWith<T>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3) }.AsSpan(), entityBuffer);
    }

    public static uint FindEntitiesWith<T1, T2, T3, T4>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6, T7>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6, T7, T8>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
        where T9 : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) }.AsSpan(), entityBuffer);
    }

    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
        where T9 : struct, IComponent
        where T10 : struct, IComponent
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10) }.AsSpan(), entityBuffer);
    }
}