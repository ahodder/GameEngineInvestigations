namespace Cyberstar.ECS;

public class ComponentMapper
{
    private Dictionary<Entity, int> _entityToIndex;

    public ComponentMapper()
    {
        _entityToIndex = new Dictionary<Entity, int>();
    }
}