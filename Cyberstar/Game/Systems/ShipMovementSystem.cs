using System.Numerics;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Cyberstar.Maths;

namespace Cyberstar.Game.Systems;

public class ShipMovementSystem : SystemBase
{
    private readonly Type[] _searchType = new[] { typeof(ShipComponent), typeof(ShipControlComponent), typeof(TransformComponent), typeof(VelocityComponent) };
    
    public override void Update(FrameTiming deltaTime)
    {
        Span<Entity> entities = stackalloc Entity[32];
        var found = 0u;

        do
        {
            found = EntityManager.FindEntitiesWith(_searchType, entities);

            for (var i = 0; i < found; i++)
            {
                ref var ship = ref EntityManager.GetComponentFor<ShipComponent>(entities[i]);
                ref var shipControl = ref EntityManager.GetComponentFor<ShipControlComponent>(entities[i]);
                ref var transform = ref EntityManager.GetComponentFor<TransformComponent>(entities[i]);
                ref var velocity = ref EntityManager.GetComponentFor<VelocityComponent>(entities[i]);

                var actualVelocity = velocity.Velocity + ship.Acceleration * shipControl.MovementDirection * deltaTime.DeltaTime;
                var lenSquared = actualVelocity.LengthSquared();
                if (lenSquared > ship.MaximumVelocity)
                    actualVelocity = actualVelocity.Normalize() * ship.MaximumVelocity;

                if (lenSquared < 1e-7)
                    actualVelocity = Vector2.Zero;

                var actualRotationalVelocity = velocity.RotationalVelocity + ship.RotationalAcceleration * shipControl.RotationDirection * deltaTime.DeltaTime;
                if (actualRotationalVelocity > ship.MaximumRotationalVelocity)
                    actualRotationalVelocity = ship.MaximumRotationalVelocity;
                
                transform.Rotate(actualRotationalVelocity);
                transform.TranslateForward(actualVelocity);

                var dragCoefficient = 1 - velocity.Drag;

                velocity.Velocity = actualVelocity * dragCoefficient;
                velocity.RotationalVelocity = actualRotationalVelocity * dragCoefficient;
            }
        } while (found >= entities.Length);
    }
}