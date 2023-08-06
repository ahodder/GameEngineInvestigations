using Cyberstar.ECS;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.UI;
using Cyberstar.UI.EcsRendering.ComponentRendering;

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
    
    public bool TryCreateDebugView(AssetManager assetManager, 
        Entity entity,
        EntityManager entityManager, 
        out ViewBase outView)
    {
        outView = new ComponentRenderer<ShipComponent>(assetManager, entity, entityManager);
        return true;
    }
}