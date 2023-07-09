using System.Numerics;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Raylib_cs;

namespace Cyberstar.Game.Systems;

public class EntityOriginRendererSystem : ComponentIteratorSystemBase
{
    public EntityOriginRendererSystem() : base(new [] { typeof(TransformComponent) })
    {
    }

    protected override void HandleEntities(ReadOnlySpan<Entity> entities, uint count)
    {
        for (var i = 0; i < count; i++)
        {
            var entity = entities[i];
            ref var transform = ref EntityManager.GetComponentFor<TransformComponent>(entity);
            Raylib.DrawSphere(new Vector3(transform.Position, 0), 50f, Color.RED);            
        }
    }
}