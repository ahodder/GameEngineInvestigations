using System.Numerics;
using Cyberstar.ECS;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.Extensions.IO;
using Cyberstar.UI;
using Cyberstar.UI.EcsRendering.ComponentRendering;

namespace Cyberstar.Game.Components;

public struct ShipControlComponent : IComponent
{
    public Vector2 MovementDirection;
    public float RotationDirection;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(MovementDirection);
        writer.Write(RotationDirection);
    }

    public void Deserialize(BinaryReader reader)
    {
        MovementDirection = reader.ReadVector2();
        RotationDirection = reader.ReadSingle();
    }
    
    public bool TryCreateDebugView(AssetManager assetManager, 
        Entity entity,
        EntityManager entityManager, 
        out ViewBase outView)
    {
        outView = new ComponentRenderer<ShipControlComponent>(assetManager, entity, entityManager);
        return true;
    }
}