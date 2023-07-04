using Cyberstar.ECS;

namespace Cyberstar.Tests.ECS;

public struct FloatComponent : IComponent
{
    public float Float;

    public FloatComponent(float f)
    {
        Float = f;
    }

    public void Serialize(BinaryWriter writer)
    {
    }

    public void Deserialize(BinaryReader reader)
    {
    }
}