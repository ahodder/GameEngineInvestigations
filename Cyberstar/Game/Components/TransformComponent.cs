using System.Numerics;
using Cyberstar.Engine.ECS;
using Cyberstar.Extensions.IO;
using Cyberstar.Maths;

namespace Cyberstar.Game.Components;

public struct TransformComponent : IComponent
{
    public Entity Parent;
    public Vector2 Position;
    public Vector2 Scale;
    public float RotationRadians;

    public Vector2 Forward => new Vector2(MathF.Cos(RotationRadians), MathF.Sin(RotationRadians));
    
    /// <summary>
    /// Gets the computed transform of this local transform. 
    /// </summary>
    /// <param name="entityManager"></param>
    /// <param name="outOffset"></param>
    /// <param name="outScale"></param>
    /// <param name="outRotation"></param>
    /// <returns></returns>
    public bool TryGetComputedValues(EntityManager entityManager, out Vector2 outOffset, out Vector2 outScale, out float outRotation)
    {
        if (entityManager.TryGetComponentFor<TransformComponent>(Parent, out var transform))
        {
            transform.TryGetComputedValues(entityManager, out var localOffset, out var localScale, out var localRotation);
            outOffset = localOffset + Position;
            outScale = localScale + Scale;
            outRotation = localRotation + RotationRadians;
            return true;
        }

        outOffset = Position;
        outScale = Scale;
        outRotation = RotationRadians;
        return false;
    }

    public void Translate(Vector2 movement)
    {
        Position += movement;
    }

    public void TranslateForward(Vector2 movement)
    {
        Position += movement.Rotate(RotationRadians);
    }

    public void Rotate(float degrees)
    {
        RotationRadians += degrees * MathF.PI / 180;
    }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Parent);
        writer.Write(Position);
        writer.Write(Scale);
        writer.Write(RotationRadians);
    }

    public void Deserialize(BinaryReader reader)
    {
        Parent = reader.ReadEntity();
        Position = reader.ReadVector2();
        Scale = reader.ReadVector2();
        RotationRadians = reader.ReadSingle();
    }
    
    public static TransformComponent FromTranslation(Vector2 translation)
    {
        var ret = new TransformComponent();
        ret.Translate(translation);
        return ret;
    }
}