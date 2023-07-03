using Cyberstar.ECS;

namespace Cyberstar.Game.Components;

public struct SpriteComponent : IComponent
{
    public string SpriteAtlas;
    public string SpriteAnimationPath;
    public int SpriteCurrentFrame;

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
}