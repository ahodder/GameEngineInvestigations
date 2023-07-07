using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.Logging;
using Cyberstar.UI;
using Cyberstar.UI.ViewFragments;
using Raylib_cs;

namespace Cyberstar.Scenes;

public class ShipBuilderScene : Scene
{
    private UiRenderer _uiRenderer;
    
    public ShipBuilderScene(ILogger logger, AssetManager assets) : base(logger, assets)
    {
        assets.TryLoadSpriteAtlas("dev_ships", out var spriteAtlas);
        var atlasView = new SpriteAtlasGridView(assets, spriteAtlas);
        atlasView.ColumnSpacing = 10;
        atlasView.RowSpacing = 10;
        atlasView.CellWidth = 50;
        atlasView.CellHeight = 50;
        atlasView.Columns = 4;
        atlasView.Rows = 4;
        atlasView.OnCellClick = (x, y) => _logger.Debug($"Grid Click: {x}, {y}");
        atlasView.OnHover = (x, y) => true;
        
        _uiRenderer = new UiRenderer(atlasView, 0, 0, 1200, 800);
    }

    public override void PerformTick(FrameTiming frameTiming)
    {
        RenderUi();
    }

    public void RenderUi()
    {
        _uiRenderer.Render();
    }
}