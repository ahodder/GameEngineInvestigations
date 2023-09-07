using System.Numerics;
using Cyberstar.Core;
using Cyberstar.Engine.ECS;
using Cyberstar.Game.Components;
using Cyberstar.Maths;

namespace Cyberstar.Game.Systems;

public class ShipMovementSystem : ComponentIteratorSystemBase
{
    public ShipMovementSystem() : base(new[] { typeof(ShipComponent), typeof(ShipControlComponent), typeof(TransformComponent), typeof(VelocityComponent) })
    {
    }

    protected override void HandleEntities(in FrameTiming frameTiming, ReadOnlySpan<Entity> entities, uint count)
    {
        for (var i = 0; i < count; i++)
        {
            ref var ship = ref EntityManager.GetComponentFor<ShipComponent>(entities[i]);
            ref var shipControl = ref EntityManager.GetComponentFor<ShipControlComponent>(entities[i]);
            ref var transform = ref EntityManager.GetComponentFor<TransformComponent>(entities[i]);
            ref var velocity = ref EntityManager.GetComponentFor<VelocityComponent>(entities[i]);

            var actualVelocity = velocity.Velocity + ship.Acceleration * shipControl.MovementDirection * frameTiming.DeltaTime;
            var lenSquared = actualVelocity.LengthSquared();
            if (lenSquared > ship.MaximumVelocity)
                actualVelocity = actualVelocity.Normalize() * ship.MaximumVelocity;

            if (lenSquared < 1e-7)
                actualVelocity = Vector2.Zero;

            var actualRotationalVelocity = velocity.RotationalVelocity + ship.RotationalAcceleration * shipControl.RotationDirection * frameTiming.DeltaTime;
            if (actualRotationalVelocity > ship.MaximumRotationalVelocity)
                actualRotationalVelocity = ship.MaximumRotationalVelocity;
            
            transform.Rotate(actualRotationalVelocity);
            transform.TranslateForward(actualVelocity);

            var dragCoefficient = 1 - velocity.Drag;

            velocity.Velocity = actualVelocity * dragCoefficient;
            velocity.RotationalVelocity = actualRotationalVelocity * dragCoefficient;
        }
    }
}