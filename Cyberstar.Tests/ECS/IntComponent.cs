using Cyberstar.Engine.AssetManagement;
using Cyberstar.Engine.ECS;
using Cyberstar.Engine.UI;

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
    
    public bool TryCreateDebugView(AssetManager assetManager, Entity entity, EntityManager entityManager, out ViewBase outView)
    {
        outView = default;
        return false;
    }
}