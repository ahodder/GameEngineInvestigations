using System.Numerics;
using System.Reflection;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.UI.EcsRendering.ComponentRendering.FieldRenderers;
using Color = Raylib_cs.Color;

namespace Cyberstar.UI.EcsRendering.ComponentRendering;

public class ComponentRenderer<T> : VerticalLayoutView where T : struct, IComponent
{
    private readonly Entity _entity;
    private readonly ComponentAllocator<T> _componentAllocator;
    private readonly List<FieldReader<T>> _fields;
    private readonly List<EntryView> _entryViews;

    public ComponentRenderer(AssetManager assetManager, Entity entity, EntityManager entityManager) : base(assetManager)
    {
        _entity = entity;
        _componentAllocator = entityManager.GetAllocatorFor<T>().Data;
        _fields = new List<FieldReader<T>>();
        _entryViews = new List<EntryView>();
        HorizontalMeasure = EAxisMeasure.FillParent;
        BackgroundColor = Color.RED;

        var componentType = typeof(T);
        foreach (var field in componentType.GetFields())
            AddViewsForField(this, field);
    }

    private void AddViewsForField(VerticalLayoutView view, FieldInfo field)
    {
        var fieldType = field.FieldType;
        FieldReader<T> reader;
            
        if (fieldType == typeof(bool))
            CreateViewsForTerminalType(view, field, new BoolFieldReader<T>(field.Name));
        else if (fieldType == typeof(short))
            CreateViewsForTerminalType(view, field, new ShortFieldReader<T>(field.Name));
        else if (fieldType == typeof(ushort))
            CreateViewsForTerminalType(view, field, new UshortFieldReader<T>(field.Name));
        else if (fieldType == typeof(int))
            CreateViewsForTerminalType(view, field, new IntFieldReader<T>(field.Name));
        else if (fieldType == typeof(uint))
            CreateViewsForTerminalType(view, field, new UintFieldReader<T>(field.Name));
        else if (fieldType == typeof(float))
            CreateViewsForTerminalType(view, field, new FloatFieldReader<T>(field.Name));
        else if (fieldType == typeof(double))
            CreateViewsForTerminalType(view, field, new DoubleFieldReader<T>(field.Name));
        else if (fieldType == typeof(string))
            CreateViewsForTerminalType(view, field, new TextFieldReader<T>(field.Name));
        else if (fieldType == typeof(Vector2))
        {
            var nestedLayout = new VerticalLayoutView(AssetManager)
            {
                HorizontalMeasure = EAxisMeasure.FillParent,
            };
            
            nestedLayout.AddView(new LabelView(AssetManager)
            {
                Tag = field.Name,
                Text = field.Name,
                FontSize = 20,
                TextColor = Color.BLACK,
                HorizontalMeasure = EAxisMeasure.FillParent,
            });
            
            foreach (var nestedField in fieldType.GetFields())
                AddViewsForField(nestedLayout, nestedField);
        }
    }

    private void CreateViewsForTerminalType(VerticalLayoutView verticalLayoutView, FieldInfo field, FieldReader<T> fieldReader)
    {
        var entryView = new EntryView(AssetManager)
        {
            Tag = "Entry " + field.Name,
            FontSize = 20,
            TextColor = Color.BLACK,
            HorizontalMeasure = EAxisMeasure.FillParent,
        };
        _entryViews.Add(entryView);
            
        AddChild(new LabelView(AssetManager)
        {
            Tag = "Label " + field.Name,
            Text = field.Name,
            FontSize = 20,
            TextColor = Color.BLACK,
            HorizontalMeasure = EAxisMeasure.FillParent,
        });
        AddChild(entryView);
        
        _fields.Add(fieldReader);
    }

    protected override void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        var component = _componentAllocator.Get(_entity);
        Span<char> updateStack = stackalloc char[128];

        for (var i = 0; i < _entryViews.Count; i++)
        {
            var count = _fields[i].GetStringValue(ref component, updateStack);
            _entryViews[i].SetText(updateStack.Slice(0, count));
        }
        
        base.DoRenderContent(in frameTiming, in inputData);
    }
}