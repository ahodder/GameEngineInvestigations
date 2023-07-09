using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Raylib_cs;

namespace Cyberstar.Game.Systems;

public class TextureFollowMouseSystem : SystemBase
{
    private readonly Type[] _searchTypes = new[] { typeof(TransformComponent), typeof(FollowMouseComponent) };

    public override void Update(FrameTiming deltaTime)
    {
        var offset = 0u;
        Span<Entity> entities = stackalloc Entity[32];
        var found = 0u;

        var mousePosition = Raylib.GetMousePosition();

        do
        {
            found = EntityManager.FindEntitiesWith(_searchTypes, entities);

            for (var i = 0; i < found; i++)
            {
                ref var transform = ref EntityManager.GetComponentFor<TransformComponent>(entities[i]);
                transform.Position = mousePosition;
            }

            offset += found;
        } while (found >= entities.Length);
    }
}