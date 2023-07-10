using System.Numerics;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Raylib_cs;

namespace Cyberstar.Game.Systems;

public class MainPlayerShipInputSystem : ComponentIteratorSystemBase
{
    public MainPlayerShipInputSystem() : base(new Type[] { typeof(MainPlayerComponent), typeof(ShipControlComponent) })
    {
    }

    protected override void HandleEntities(in FrameTiming frameTiming, ReadOnlySpan<Entity> entities, uint count)
    {
        for (var i = 0; i < count; i++)
        {
            ref var shipControl = ref EntityManager!.GetComponentFor<ShipControlComponent>(entities[i]);

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
    }
}