using Cyberstar.Core;

namespace Cyberstar.Engine.ECS;

public interface ISystem
{ 
    bool Enabled { get; set; }
    
    EntityManager? EntityManager { get; set; }
    
    void PreUpdate();
    void Update(in FrameTiming frameTiming);
    void PostUpdate();
}