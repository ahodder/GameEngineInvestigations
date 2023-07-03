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
                ref var sprite = ref EntityManager.GetComponentFor<SpriteComponent>(entities[i]);
                ref var transform = ref EntityManager.GetComponentFor<TransformComponent>(entities[i]);

                if (!_assetManager.GetSpriteFromAtlas(sprite.SpriteAtlas, sprite.SpriteAnimationPath, sprite.SpriteCurrentFrame, out var texture, out var frame))
                    continue;

                var pos = transform.Position;
                Raylib.DrawTexturePro(texture,
                    frame,
                    new Rectangle(pos.X, pos.Y, frame.width, frame.height),
                    new Vector2(frame.width / 2f, frame.height / 2f),
                    transform.RotationRadians * 180 / MathF.PI,
                    Color.WHITE);
            }

            offset += found;
        } while (found >= entities.Length);
    }
}