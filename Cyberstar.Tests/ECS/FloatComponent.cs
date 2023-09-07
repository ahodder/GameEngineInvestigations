using Cyberstar.Engine.AssetManagement;
using Cyberstar.Engine.ECS;
using Cyberstar.Engine.UI;

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

    public bool TryCreateDebugView(AssetManager assetManager, Entity entity, EntityManager entityManager, out ViewBase outView)
    {
        outView = default;
        return false;
    }
}