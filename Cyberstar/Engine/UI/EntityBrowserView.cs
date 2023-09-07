using Cyberstar.Engine.AssetManagement;
using Cyberstar.Engine.ECS;
using Point = System.Drawing.Point;

namespace Cyberstar.Engine.UI;

public class EntityBrowserView : VerticalLayoutView
{
    public EntityManager EntityManager { get; }

    public Entity Entity
    {
        get => _entity;
        set
        {
            _entity = value;
            PopulateViews();
        }
    }

    private readonly List<IComponent> _components = new List<IComponent>();
    private Entity _entity;
    
    public EntityBrowserView(AssetManager assetManager, EntityManager entityManager) : base(assetManager)
    {
        EntityManager = entityManager;
        AddChild(new LabelView(AssetManager, $"Entity {Entity.ToString()}")
        {
            Tag = "Header",
            Padding = new Thickness(5),
        });
    }

    public void PopulateViews()
    {
        RemoveAllChildren();
        _components.Clear();
        
        AddChild(new LabelView(AssetManager, $"Entity {Entity.ToString()}")
        {
            Tag = "Header",
            Padding = new Thickness(5),
        });

        EntityManager.TryGetAllComponents(Entity, _components);

        foreach (var component in _components)
        {
            // if (component.TryCreateDebugView(AssetManager, Entity, EntityManager, out var view))
            // {
            //     AddChild(view);
            // }
        }

        NeedsRemeasure = true;
    }

    protected override Point MeasureSelf(int x, int y, int width, int height)
    {
        var maxWidth = 0;
        var offset = y;
        
        foreach (var view in Children)
        {
            view.MeasureAndLayout(x, offset, width, height);
            maxWidth = Math.Max(maxWidth, view.Bounds.Width);
            offset += view.Bounds.Height;
        }

        return new Point(maxWidth, offset);
    }
}