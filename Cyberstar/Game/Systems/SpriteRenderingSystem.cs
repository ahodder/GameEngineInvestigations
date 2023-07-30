using System.Numerics;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.Game.Components;
using Raylib_cs;

namespace Cyberstar.Game.Systems;

public class SpriteRenderingSystem : ComponentIteratorSystemBase
{

    private readonly AssetManager _assetManager;

    public SpriteRenderingSystem(AssetManager assetManager) : base(new[] { typeof(SpriteComponent), typeof(TransformComponent) })
    {
        _assetManager = assetManager;
    }

    protected override void HandleEntities(in FrameTiming frameTiming, ReadOnlySpan<Entity> entities, uint count)
    {
        for (var i = 0; i < count; i++)
        {
            ref var sprite = ref EntityManager!.GetComponentFor<SpriteComponent>(entities[i]);
            ref var transform = ref EntityManager.GetComponentFor<TransformComponent>(entities[i]);

            if (!_assetManager.TryGetSpriteFromAtlas(sprite.SpriteAtlas, sprite.SpriteAnimationPath, sprite.SpriteCurrentFrame, out var texture, out var frame))
                continue;

            var pos = transform.Position;
            Raylib.DrawTexturePro(texture,
                frame,
                new Rectangle(pos.X, pos.Y, frame.width, frame.height),
                new Vector2(frame.width / 2f, frame.height / 2f),
                transform.RotationRadians * 180 / MathF.PI,
                Color.WHITE);
        }
    }
}