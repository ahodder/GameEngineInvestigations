using System.Drawing;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Color = Raylib_cs.Color;

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

    public LabeledExpanderView(AssetManager assetManager, IView expandedView) : base(assetManager)
    {
        Header = new ButtonView(assetManager);
        Header.IsEnabled = true;
        Header.Padding = new Thickness().Set(15);
        Header.Margin = new Thickness().Set(15);
        Header.Text = "Unlabeled Header";
        Header.BackgroundColor = Color.GRAY;
        Header.FontSize = 18;

        ExpandedView = expandedView;
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        var totalWidth = int.MaxValue;
        var totalHeight = y;
        
        Header.Measure(x, y, width, height);
        totalWidth = Math.Min(width, Header.MeasuredSize.X);
        totalHeight += Header.Bounds.Height;

        if (ExpandedView.IsEnabled)
        {
            ExpandedView.Measure(x, totalHeight, width, height);
            totalWidth = Math.Min(width, ExpandedView.MeasuredSize.X);
            totalHeight += ExpandedView.Bounds.Height;
        }

        return new Point(totalWidth, totalHeight);
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