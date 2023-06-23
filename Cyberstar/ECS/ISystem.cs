using Cyberstar.Core;

namespace Cyberstar.ECS;

public interface ISystem
{
    public bool Enabled { get; set; }
    
    void PreUpdate();
    void Update(FrameTiming time);
    void PostUpdate();
}