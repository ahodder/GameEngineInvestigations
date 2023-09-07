using System.Numerics;
using Cyberstar.Engine.ECS;

namespace Cyberstar.Extensions.IO;

public static class BinaryReaderExtensions
{
    public static Entity ReadEntity(this BinaryReader reader) => new(reader.ReadInt32(), reader.ReadInt32());
    public static Vector2 ReadVector2(this BinaryReader reader) => new(reader.ReadSingle(), reader.ReadSingle());
}