namespace Cyberstar.Engine.ECS;

public interface IComponent
{
    void Serialize(BinaryWriter writer);
    void Deserialize(BinaryReader reader);
}