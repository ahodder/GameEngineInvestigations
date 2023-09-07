using System.Numerics;
using Cyberstar.Engine.ECS;
using Cyberstar.Extensions.IO;

namespace Cyberstar.Game.Components;

public struct VelocityComponent : IComponent
{
    public Vector2 Velocity;
    public float RotationalVelocity;
    public float Drag;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Velocity);
        writer.Write(RotationalVelocity);
        writer.Write(Drag);
    }

    public void Deserialize(BinaryReader reader)
    {
        Velocity = reader.ReadVector2();
        RotationalVelocity = reader.ReadSingle();
        Drag = reader.ReadSingle();
    }
}