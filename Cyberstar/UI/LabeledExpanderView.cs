using System.Drawing;
using Cyberstar.Core;
using Cyberstar.Engine.AssetManagement;

namespace Cyberstar.UI;

public class LabeledExpanderView : ViewBase
{
    public ButtonView Header { get; }
    public IView ExpandedView { get; }
    
    /// <summary>
    /// Whether or not the Expanded view should be rendered.
    /// </summary>
    public bool IsExpanded
    {
        get => ExpandedView.IsEnabled;
        set => ExpandedView.IsEnabled = value;
    }

    public LabeledExpanderView(AssetManager assetManager, IView expandedView, ReadOnlySpan<char> text) : base(assetManager)
    {
        Header = new ButtonView(assetManager);
        Header.IsEnabled = true;
        Header.Padding = new Thickness().Set(15);
        Header.Margin = new Thickness().Set(15);
        Header.Text = text;
        Header.FontSize = 18;

        ExpandedView = expandedView;
    }

    protected override Point MeasureSelf(int x, int y, int width, int height)
    {
        var maxWidth = 0;
        var offset = 0;
        
        Header.MeasureAndLayout(x, y, width, height);
        maxWidth = Math.Max(Header.Bounds.Width, maxWidth);
        offset += Header.Bounds.Height;

        if (ExpandedView.IsEnabled)
        {
            ExpandedView.MeasureAndLayout(x, y + offset, width, height);
            maxWidth = Math.Min(maxWidth, ExpandedView.Bounds.Width);
            offset += ExpandedView.Bounds.Height;
        }

        return new Point(maxWidth, offset);
    }

    protected override void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        ExpandedView.Render(in frameTiming, in inputData);
        Header.Render(in frameTiming, in inputData);
    }

    public override bool WillHandleMouseClick(int mouseX, int mouseY)
    {
        if (Header.WillHandleMouseClick(mouseX, mouseY))
            return true;
        
        if (ExpandedView.IsEnabled && ExpandedView.WillHandleMouseClick(mouseX, mouseY))
            return true;

        return false;
    }
}