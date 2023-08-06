using System.Numerics;
using Cyberstar.ECS;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.Extensions.IO;
using Cyberstar.UI;
using Cyberstar.UI.EcsRendering.ComponentRendering;

namespace Cyberstar.Game.Components;

public struct VelocityComponent : IComponent
{
    public Vector2 Velocity;
    public float RotationalVelocity;
    public float Drag;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Velocity);
        writer.Write(RotationalVelocity);
        writer.Write(Drag);
    }

    public void Deserialize(BinaryReader reader)
    {
        Velocity = reader.ReadVector2();
        RotationalVelocity = reader.ReadSingle();
        Drag = reader.ReadSingle();
    }
    
    public bool TryCreateDebugView(AssetManager assetManager, 
        Entity entity,
        EntityManager entityManager, 
        out ViewBase outView)
    {
        outView = new ComponentRenderer<VelocityComponent>(assetManager, entity, entityManager);
        return true;
    }
}