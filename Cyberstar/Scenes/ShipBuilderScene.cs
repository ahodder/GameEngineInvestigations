using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.Logging;
using Cyberstar.Sprites;
using Cyberstar.UI;
using Raylib_cs;

namespace Cyberstar.Scenes;

public class ShipBuilderScene : Scene
{
    private SpriteAtlas _shipFrameSpriteAtlas;
    
    private TexturePreviewView _selectedFramePreview;
    private UiRenderer _uiRenderer;

    private Sprite? _spriteFollowingMouse;
    
    public ShipBuilderScene(ILogger logger, WindowData windowData, AssetManager assets) : base(logger, windowData, assets)
    {
        assets.TryLoadSpriteAtlas("dev_ships", out _shipFrameSpriteAtlas);
        var atlasView = new SpriteAtlasGridView(assets, _shipFrameSpriteAtlas);
        atlasView.ColumnSpacing = 10;
        atlasView.RowSpacing = 10;
        atlasView.CellWidth = 50;
        atlasView.CellHeight = 50;
        atlasView.Columns = 4;
        atlasView.Rows = 4;
        atlasView.OnCellClick = SelectedShipFrame;
        atlasView.OnHover = (x, y) => true;

        var spriteCollection = new LabeledExpanderView(assets, atlasView);
        spriteCollection.Header.Text = "Ship Frames";
        spriteCollection.Header.OnClick = () => spriteCollection.IsExpanded = !spriteCollection.IsExpanded;

        var currentBuiltShipComponents = new VerticalLayoutView(assets);
        currentBuiltShipComponents.AddView(new LabelView(assets, assets.FontAtlas.DefaultFont, 18, 1, Color.BLACK, Color.GRAY));
        
        var absoluteLayout = new AbsoluteLayout(assets);
        absoluteLayout.AddView(spriteCollection, 0, 0, 400, windowData.Height);
        
        
        _uiRenderer = new UiRenderer(absoluteLayout, 0, 0, windowData.Width, windowData.Height);
    }

    private void SelectedShipFrame(Sprite selectedSprite)
    {
        if (_spriteFollowingMouse == selectedSprite)
            _spriteFollowingMouse = null;
        else
            _spriteFollowingMouse = selectedSprite;
    }

    public override void PerformTick(FrameTiming frameTiming)
    {
        RenderUi();
    }

    public void RenderUi()
    {
        _uiRenderer.Render();

        if (_spriteFollowingMouse != null)
        {

            var frame = _spriteFollowingMouse.Frames[0].Rectangle;
            var mousePosition = Raylib.GetMousePosition();

            var dest = new Rectangle(mousePosition.X - frame.width / 2, mousePosition.Y - frame.height / 2, frame.width, frame.height);
            
            Raylib.DrawTexturePro(_shipFrameSpriteAtlas.BackingTexture,
                frame,
                dest,
                Vector2.Zero, 
                0,
                Color.WHITE);
        }
    }
}