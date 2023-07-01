using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Raylib_cs;

namespace Cyberstar.Game.Systems;

public class SpriteRenderingSystem : SystemBase
{
    private readonly Type[] _searchTypes = new[] { typeof(SpriteComponent), typeof(TransformComponent) };

    private readonly AssetManager _assetManager;

    public SpriteRenderingSystem(AssetManager assetManager)
    {
        _assetManager = assetManager;
    }
    
    public override void Update(FrameTiming deltaTime)
    {
        var offset = 0u;
        Span<Entity> entities = stackalloc Entity[32];
        var found = 0u;

        do
        {
            found = EntityManager.FindEntitiesWith(_searchTypes, entities);

            for (var i = 0; i < found; i++)
            {
                EntityManager.TryGetComponentFor(entities[i], out SpriteComponent sprite);
                EntityManager.TryGetComponentFor(entities[i], out TransformComponent transform);

                if (!_assetManager.GetSpriteFromAtlas(sprite.SpriteAtlas, sprite.SpriteAnimationPath, sprite.SpriteCurrentFrame, out var texture, out var frame))
                    continue;

                Raylib.DrawTextureRec(texture, frame, new Vector2(transform.X, transform.Y), Color.WHITE);
            }

            offset += found;
        } while (found >= entities.Length);
    }
}