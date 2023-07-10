using System.Drawing;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.ECS;

namespace Cyberstar.UI;

public class EntityHierarchyView : ViewBase
{
    private EntityManager _entityManager;
    
    public EntityHierarchyView(AssetManager assetManager, EntityManager entityManager) : base(assetManager)
    {
        _entityManager = entityManager;
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        throw new NotImplementedException();
    }

    protected override void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        throw new NotImplementedException();
    }
}