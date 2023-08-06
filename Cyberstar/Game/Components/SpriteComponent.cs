using Cyberstar.ECS;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.UI;
using Cyberstar.UI.EcsRendering.ComponentRendering;

namespace Cyberstar.Game.Components;

public struct SpriteComponent : IComponent
{
    public string SpriteAtlas;
    public string SpriteAnimationPath;
    public int SpriteCurrentFrame;

    public SpriteComponent(string spriteAtlas, string spriteAnimationPath, int spriteCurrentFrame)
    {
        SpriteAtlas = spriteAtlas;
        SpriteAnimationPath = spriteAnimationPath;
        SpriteCurrentFrame = spriteCurrentFrame;
    }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(SpriteAtlas);
        writer.Write(SpriteAnimationPath);
        writer.Write(SpriteCurrentFrame);
    }

    public void Deserialize(BinaryReader reader)
    {
        SpriteAtlas = reader.ReadString();
        SpriteAnimationPath = reader.ReadString();
        SpriteCurrentFrame = reader.ReadInt32();
    }
    
    public bool TryCreateDebugView(AssetManager assetManager, 
        Entity entity,
        EntityManager entityManager, 
        out ViewBase outView)
    {
        outView = new ComponentRenderer<SpriteComponent>(assetManager, entity, entityManager);
        return true;
    }
}