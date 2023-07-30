using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.ECS;
using Raylib_cs;
using Point = System.Drawing.Point;

namespace Cyberstar.UI;

public class EntityBrowserView : VerticalLayoutView
{
    public EntityManager EntityManager { get; }

    public Dictionary<int, EntryView> EditorMapping = new Dictionary<int, EntryView>();

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
        AddView(new LabelView(AssetManager, $"Entity {Entity.ToString()}")
        {
            Tag = "Header",
            Padding = new Thickness(5),
        });
    }

    public void PopulateViews()
    {
        RemoveAllChildren();
        _components.Clear();
        
        AddView(new LabelView(AssetManager, $"Entity {Entity.ToString()}")
        {
            Tag = "Header",
            Padding = new Thickness(5),
        });

        EntityManager.TryGetAllComponents(Entity, _components);

        foreach (var component in _components)
        {
            var componentType = component.GetType();
            var fields = componentType.GetFields();
            if (fields?.Length <= 0) continue;

            var components = new VerticalLayoutView(AssetManager)
            {
                Padding = new Thickness(5),
            };

            foreach (var field in fields)
            {
                var layout = new HorizontalLayoutView(AssetManager);
                layout.BackgroundColor = Color.GOLD;
                layout.AddView(new LabelView(AssetManager, field.Name)
                {
                    Padding = new Thickness(5),
                });

                var tag = $"{componentType.Name}.{field.Name}";

                var entry = new EntryView(AssetManager)
                {
                    Tag = tag,
                    Text = field.GetValue(component).ToString(),
                };
                layout.AddView(entry);
                components.AddView(layout);

                EditorMapping[SpanHashCode(tag.AsSpan())] = entry;
            }
            
            var componentView = new LabeledExpanderView(AssetManager, components, component.GetType().Name)
            {
                Padding = new Thickness(5),
                BackgroundColor = Color.LIME,
            };
            componentView.Header.OnClick += () => componentView.IsExpanded = !componentView.IsExpanded;

            AddView(componentView);
        }

        NeedsRemeasure = true;
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        var maxWidth = 0;
        var offset = y;
        
        foreach (var view in Children)
        {
            view.Measure(x, offset, width, height);
            maxWidth = Math.Max(maxWidth, view.Bounds.Width);
            offset += view.Bounds.Height;
        }

        return new Point(maxWidth, offset);
    }

    protected override void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        UpdateViewData();
        
        foreach (var view in Children)
        {
            view.Render(in frameTiming, in inputData);
        }
    }

    public void UpdateViewData()
    {
        EntityManager.TryGetAllComponents(Entity, _components);

        Span<char> text = stackalloc char[128];
        var pointer = 0;

        foreach (var component in _components)
        {
            var componentType = component.GetType();
            var fields = componentType.GetFields();
            if (fields?.Length <= 0) continue;
            
            componentType.Name.CopyTo(text);
            pointer = componentType.Name.Length;
            text[pointer++] = '.';
            text[pointer] = '\0';

            foreach (var field in fields)
            {
                field.Name.CopyTo(text.Slice(pointer));
                var val = field.GetValue(component).ToString();
                var end = field.Name.Length;
                text[pointer + end] = '\0';
                var code = SpanHashCode(text.Slice(0, pointer + end));
                if (EditorMapping.TryGetValue(code, out var view))
                {
                    view.Text = val;
                }
            }
        }
    }
    
    private int SpanHashCode(ReadOnlySpan<char> span)
    {
        var ret = 0;

        for (var i = 0; i < span.Length; i++)
        {
            ret += span[i] * 31 ^ (span.Length - 1 - i);
        }
            
        return ret;
    }
}