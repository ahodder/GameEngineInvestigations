// using Cyberstar.AssetManagement;
// using Cyberstar.Core;
// using Cyberstar.Logging;
// using Cyberstar.UI;
// using Cyberstar.UI.ViewFragments;
//
// namespace Cyberstar.Scenes;
//
// public class ShipBuilderScene : Scene
// {
//     
//     private SpriteAtlasGridView _atlasView;
//
//     private UiRenderer _uiRenderer;
//     
//     public ShipBuilderScene(ILogger logger, AssetManager assets) : base(logger, assets)
//     {
//         _uiRenderer = new UiRenderer(assets);
//         
//         _atlasView = new SpriteAtlasGridView();
//         _atlasView.Dimensions = new ViewDimensions(0, 0, 400, 600);
//         _atlasView.ColumnSpacing = 2;
//         _atlasView.RowSpacing = 2;
//         _atlasView.Columns = 4;
//         _atlasView.Rows = 4;
//
//         assets.TryLoadSpriteAtlas("dev_ships", out var spriteAtlas);
//         _atlasView.SpriteAtlas = spriteAtlas;
//         _atlasView.OnCellClick = (x, y) => _logger.Debug($"Grid Click: {x}, {y}");
//         _atlasView.OnHover = (x, y) => true;
//     }
//
//     public override void PerformTick(FrameTiming frameTiming)
//     {
//         RenderUi();
//     }
//
//     public void RenderUi()
//     {
//         _uiRenderer.DrawSpriteGridView(_atlasView);
//     }
// }