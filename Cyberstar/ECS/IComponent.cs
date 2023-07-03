namespace Cyberstar.ECS;

public interface IComponent
{
    void Serialize(BinaryWriter writer);
    void Deserialize(BinaryReader reader);
}