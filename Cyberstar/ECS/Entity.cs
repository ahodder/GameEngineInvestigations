namespace Cyberstar.ECS;

public readonly struct Entity
{
    public readonly int Id;
    public readonly int Generation;

    public Entity(int id, int generation = 1)
    {
        Id = id;
        Generation = generation;
    }

    public Entity NewGeneration() => new Entity(Id, Generation + 1);

    public override bool Equals(object? obj)
    {
        if (obj is Entity entity)
            return entity.Id == Id && entity.Generation == Generation;
        return false;
    }

    public override int GetHashCode() => Id ^ Generation;

    public override string ToString() => $"Id-{Id} : G-{Generation}";

    public static bool operator ==(Entity left, Entity right) => left.Id == right.Id && left.Generation == right.Generation;
    
    public static bool operator !=(Entity left, Entity right) => left.Id != right.Id || left.Generation != right.Generation;
}