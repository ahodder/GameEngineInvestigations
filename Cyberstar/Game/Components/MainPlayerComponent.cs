using Cyberstar.ECS;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.UI;

namespace Cyberstar.Game.Components;

public struct MainPlayerComponent : IComponent
{
    public void Serialize(BinaryWriter writer)
    {
    }

    public void Deserialize(BinaryReader reader)
    {
    }
    
    public bool TryCreateDebugView(AssetManager assetManager, 
        Entity entity,
        EntityManager entityManager, 
        out ViewBase outView)
    {
        outView = default;
        return false;
    }
}