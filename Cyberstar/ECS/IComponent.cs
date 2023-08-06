using Cyberstar.Engine.AssetManagement;
using Cyberstar.UI;

namespace Cyberstar.ECS;

public interface IComponent
{
    void Serialize(BinaryWriter writer);
    void Deserialize(BinaryReader reader);
    bool TryCreateDebugView(AssetManager assetManager,
        Entity entity,
        EntityManager entityManager, 
        out ViewBase outView);
}