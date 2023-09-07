using System.Numerics;
using Cyberstar.Engine.ECS;
using Cyberstar.Extensions.IO;

namespace Cyberstar.Game.Components;

public struct ShipControlComponent : IComponent
{
    public Vector2 MovementDirection;
    public float RotationDirection;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(MovementDirection);
        writer.Write(RotationDirection);
    }

    public void Deserialize(BinaryReader reader)
    {
        MovementDirection = reader.ReadVector2();
        RotationDirection = reader.ReadSingle();
    }
}