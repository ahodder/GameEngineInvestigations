using System.Numerics;
using Cyberstar.ECS;

namespace Cyberstar.Game.Components;

public struct ShipComponent : IComponent
{
    public float Acceleration;
    public float MaximumVelocity;
    public float RotationalAcceleration;
    public float MaximumRotationalVelocity;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Acceleration);
        writer.Write(MaximumVelocity);
        writer.Write(RotationalAcceleration);
        writer.Write(MaximumRotationalVelocity);
    }

    public void Deserialize(BinaryReader reader)
    {
        Acceleration = reader.ReadSingle();
        MaximumVelocity = reader.ReadSingle();
        RotationalAcceleration = reader.ReadSingle();
        MaximumRotationalVelocity = reader.ReadSingle();
    }
}