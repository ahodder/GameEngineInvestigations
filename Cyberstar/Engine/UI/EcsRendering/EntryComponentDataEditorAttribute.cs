using System.Linq.Expressions;
using System.Reflection;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.Engine.ECS;

namespace Cyberstar.Engine.UI.EcsRendering;

public abstract class EntryComponentDataEditorAttribute : Attribute
{
    public abstract bool TryCreateViewFor(AssetManager assetManager, FieldInfo fieldInfo, IComponent component, out IView outView);
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class EntryComponentDataEditorAttribute<TSource, TData> : EntryComponentDataEditorAttribute
{
    public override bool TryCreateViewFor(AssetManager assetManager,
        FieldInfo fieldInfo,
        IComponent component,
        out IView outView)
    {
        var ret = new HorizontalLayoutView(assetManager);

        var label = new LabelView(assetManager);
        label.Text = fieldInfo.Name;

        var entry = new ComponentEntryView(assetManager, CreateExpressionGetter(fieldInfo));
        // entry.Update();
        
        ret.AddView(label);
        ret.AddView(entry);
        outView = ret;
        return true;
    }
    
    public static Func<TSource, TData> CreateExpressionGetter(FieldInfo fieldInfo)
    {
        var instanceParameter = Expression.Parameter(typeof(TSource), fieldInfo.Name);
        var propertyAccess = Expression.Field(instanceParameter, fieldInfo);
        var lambda = Expression.Lambda<Func<TSource, TData>>(propertyAccess, instanceParameter);
        return lambda.Compile();
    }

    public interface IEntryViewUpdater
    {
        void Update(object self);
    }

    private class ComponentEntryView : EntryView, IEntryViewUpdater
    {
        private readonly Func<TSource, TData> _componentFieldFetcher;
        
        public ComponentEntryView(AssetManager assetManager, Func<TSource, TData> componentFieldFetcher) : base(assetManager)
        {
            _componentFieldFetcher = componentFieldFetcher;
        }

        public void Update(object self)
        {
            Span<char> dest = stackalloc char[64];
            /* todo ahodder@praethos.com 9/6/23: Boxing cast? */
            var data = _componentFieldFetcher((TSource)self);
            switch (data)
            {
                case int i32:
                    if (i32.TryFormat(dest.Slice(0, dest.Length - 1), out var charsWritten))
                        dest[charsWritten] = '\0';
                    break;
                    
            }

            SetText(dest);
        }
    }
}