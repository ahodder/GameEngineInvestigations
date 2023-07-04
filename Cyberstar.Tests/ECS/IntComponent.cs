using Cyberstar.ECS;

namespace Cyberstar.Tests.ECS;

public struct IntComponent : IComponent
{
    public int Int;

    public IntComponent(int i)
    {
        Int = i;
    }

    public void Serialize(BinaryWriter writer)
    {
    }

    public void Deserialize(BinaryReader reader)
    {
    }
}