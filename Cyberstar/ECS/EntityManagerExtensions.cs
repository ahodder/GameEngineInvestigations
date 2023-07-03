using Cyberstar.Core;

namespace Cyberstar.ECS;

public static class EntityManagerExtensions
{
    public static Result<ComponentAllocator<T>> GetAllocatorFor<T>(this EntityManager entityManager) where T : struct
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
        where T1 : struct 
        where T2 : struct
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
    }
    
    public static void SetComponentsFor<T1, T2, T3>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2, T3 t3)
        where T1 : struct 
        where T2 : struct
        where T3 : struct
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
        entityManager.SetComponentFor(entity, t3);
    }
    
    public static void SetComponentsFor<T1, T2, T3, T4>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2, T3 t3, T4 t4)
        where T1 : struct 
        where T2 : struct
        where T3 : struct
        where T4 : struct
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
        entityManager.SetComponentFor(entity, t3);
        entityManager.SetComponentFor(entity, t4);
    }
    
    public static void SetComponentsFor<T1, T2, T3, T4, T5>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        where T1 : struct 
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
    {
        entityManager.SetComponentFor(entity, t1);
        entityManager.SetComponentFor(entity, t2);
        entityManager.SetComponentFor(entity, t3);
        entityManager.SetComponentFor(entity, t4);
        entityManager.SetComponentFor(entity, t5);
    }
    
    public static void SetComponentsFor<T1, T2, T3, T4, T5, T6>(this EntityManager entityManager, Entity entity,
        T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        where T1 : struct 
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
        where T6 : struct
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
        where T1 : struct 
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
        where T6 : struct
        where T7 : struct
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
        where T : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct
        where T2 : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3) }.AsSpan(), entityBuffer);
    }

    public static uint FindEntitiesWith<T1, T2, T3, T4>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
        where T6 : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6, T7>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
        where T6 : struct
        where T7 : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6, T7, T8>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
        where T6 : struct
        where T7 : struct
        where T8 : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) }.AsSpan(), entityBuffer);
    }
    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
        where T6 : struct
        where T7 : struct
        where T8 : struct
        where T9 : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) }.AsSpan(), entityBuffer);
    }

    
    public static uint FindEntitiesWith<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this EntityManager entityManager, Span<Entity> entityBuffer, uint offset = 0)
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
        where T6 : struct
        where T7 : struct
        where T8 : struct
        where T9 : struct
        where T10 : struct
    {
        /* todo ahodder@praethos.com 6/29/22: remove this allocation */
        return entityManager.FindEntitiesWith(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10) }.AsSpan(), entityBuffer);
    }
}