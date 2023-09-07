using System.Numerics;
using Cyberstar.Core;
using Cyberstar.Engine.ECS;
using Cyberstar.Game.Components;
using Raylib_cs;

namespace Cyberstar.Game.Systems;

public class TextureFollowMouseSystem : ComponentIteratorSystemBase
{
    private Vector2 _mousePosition;
    
    public TextureFollowMouseSystem() : base(new[] { typeof(TransformComponent), typeof(FollowMouseComponent) })
    {
    }

    public override void PreUpdate()
    {
        _mousePosition = Raylib.GetMousePosition();
    }

    protected override void HandleEntities(in FrameTiming frameTiming, ReadOnlySpan<Entity> entities, uint count)
    {
        for (var i = 0; i < count; i++)
        {
            ref var transform = ref EntityManager!.GetComponentFor<TransformComponent>(entities[i]);
            transform.Position = _mousePosition;
        }
    }
}