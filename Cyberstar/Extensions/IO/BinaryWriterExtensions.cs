using System.Numerics;
using Cyberstar.ECS;

namespace Cyberstar.Extensions.IO;

public static class BinaryWriterExtensions
{
    public static void Write(this BinaryWriter writer, Entity entity)
    {
        writer.Write(entity.Id);
        writer.Write(entity.Generation);
    }

    public static void Write(this BinaryWriter writer, Vector2 vector2)
    {
        writer.Write(vector2.X);
        writer.Write(vector2.Y);
    }
}