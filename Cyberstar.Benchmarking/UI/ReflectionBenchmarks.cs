using BenchmarkDotNet.Attributes;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.UI;
using Cyberstar.UI.EcsRendering.ComponentRendering;
using Cyberstar.UI.EcsRendering.ComponentRendering.FieldRenderers;

namespace Cyberstar.Benchmarking.UI;

[MemoryDiagnoser]
public class ReflectionBenchmarks
{
    public struct FloatComponent : IComponent
    {
        public float Float;

        public FloatComponent(float value)
        {
            Float = value;
        }

        public void Serialize(BinaryWriter writer)
        {
        }

        public void Deserialize(BinaryReader reader)
        {
        }

        public bool TryCreateDebugView(AssetManager assetManager, Entity entity, EntityManager entityManager, out ViewBase outView)
        {
            outView = default;
            return false;
        }
    }

    private EntityManager _entityManager;
    private ComponentRenderer<FloatComponent> _renderer;

    public ReflectionBenchmarks()
    {
        _entityManager = new EntityManager(null, 16);
        var entity = _entityManager.CreateEntity();
        _entityManager.SetComponentFor(entity, new FloatComponent(77));

        var assetManager = new AssetManager(null, "");

        _renderer = new ComponentRenderer<FloatComponent>(assetManager, entity, _entityManager);
        _renderer.MeasureAndLayout(0, 0, 100, 100);
    }

    [Benchmark]
    public void BenchmarkFloatComponent()
    {
        var ft = new FrameTiming();
        var id = new InputData();
        _renderer.Render(in ft, in id);
    }
}