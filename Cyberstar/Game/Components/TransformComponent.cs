using System.Numerics;
using Cyberstar.ECS;
using Cyberstar.Extensions.IO;
using Cyberstar.Maths;

namespace Cyberstar.Game.Components;

public struct TransformComponent : IComponent
{
    public Vector2 Position { get; private set; }
    public Vector2 Scale { get; private set; }
    public float RotationRadians { get; private set; }

    public Vector2 Forward => new Vector2(MathF.Cos(RotationRadians), MathF.Sin(RotationRadians));

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
        writer.Write(Position);
        writer.Write(Scale);
        writer.Write(RotationRadians);
    }

    public void Deserialize(BinaryReader reader)
    {
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