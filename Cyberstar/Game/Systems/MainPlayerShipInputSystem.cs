using System.Numerics;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Raylib_cs;

namespace Cyberstar.Game.Systems;

public class MainPlayerShipInputSystem : SystemBase
{
    private readonly Type[] _searchType = new[] { typeof(MainPlayerComponent), typeof(ShipControlComponent) };

    public override void Update(FrameTiming deltaTime)
    {
        Span<Entity> entities = stackalloc Entity[32];
        var found = 0u;

        do
        {
            found = EntityManager.FindEntitiesWith(_searchType, entities);

            for (var i = 0; i < found; i++)
            {
                ref var shipControl = ref EntityManager.GetComponentFor<ShipControlComponent>(entities[i]);

                var movementDirection = new Vector2();
                var rotation = 0;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_A)) movementDirection.X -= 1f;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_D)) movementDirection.X += 1f;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_W)) movementDirection.Y -= 1f;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_S)) movementDirection.Y += 1f;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_Q)) rotation -= 1;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_E)) rotation += 1;

                shipControl.MovementDirection = movementDirection;
                shipControl.RotationDirection = rotation;
            }
        } while (found >= entities.Length);
    }
}