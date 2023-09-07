using System.Numerics;
using Cyberstar.Core;
using Cyberstar.Engine.ECS;
using Cyberstar.Game.Components;
using Raylib_cs;

namespace Cyberstar.Game.Systems;

public class EntityOriginRendererSystem : ComponentIteratorSystemBase
{
    public EntityOriginRendererSystem() : base(new [] { typeof(TransformComponent) })
    {
    }

    protected override void HandleEntities(in FrameTiming frameTiming, ReadOnlySpan<Entity> entities, uint count)
    {
        for (var i = 0; i < count; i++)
        {
            var entity = entities[i];
            ref var transform = ref EntityManager!.GetComponentFor<TransformComponent>(entity);
            Raylib.DrawCircle((int)transform.Position.X, (int)transform.Position.Y, 5f, Color.RED);            
        }
    }
}