using System.Drawing;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.ECS;
using Cyberstar.Game.Components;
using Cyberstar.Game.Systems;
using Cyberstar.Logging;
using Cyberstar.Sprites;
using Cyberstar.UI;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Cyberstar.Scenes;

public class ShipBuilderScene : Scene
{
    private SpriteAtlas _spriteAtlas;

    private TexturePreviewView _selectedFramePreview;
    private EntityBrowserView _entityBrowserView;
    private UiRenderer _uiRenderer;
    private EntityManager _entityManager;

    private Entity? _activeEntity;

    public ShipBuilderScene(ILogger logger, WindowData windowData, AssetManager assets) : base(logger, windowData, assets)
    {
        _entityManager = new EntityManager(logger, 16);
        _entityManager.AddSystem(new TextureFollowMouseSystem());
        _entityManager.AddSystem(new SpriteRenderingSystem(assets));
        _entityManager.AddSystem(new EntityOriginRendererSystem());
        
        assets.TryLoadSpriteAtlas("dev_ships", out _spriteAtlas);
        var atlasView = new SpriteAtlasGridView(assets, _spriteAtlas);
        atlasView.ColumnSpacing = 10;
        atlasView.RowSpacing = 10;
        atlasView.CellWidth = 50;
        atlasView.CellHeight = 50;
        atlasView.Columns = 4;
        atlasView.Rows = 4;
        atlasView.OnCellClick = SelectedShipFrame;
        atlasView.OnHover = (x, y) => true;

        var spriteCollection = new LabeledExpanderView(assets, atlasView, "Ship Frames");
        spriteCollection.Header.OnClick = () => spriteCollection.IsExpanded = !spriteCollection.IsExpanded;

        var absoluteLayout = new AbsoluteLayout(assets);
        absoluteLayout.AddView(spriteCollection, 0, 0, 400, windowData.Height);

        var entry = new EntryView(assets, assets.FontAtlas.DefaultFont, 18, .5f);
        entry.BackgroundColor = Color.GOLD;
        entry.RequestedSize = new Point(150, 20);
        entry.Padding = new Thickness(5);
        entry.Text = "Hello, ";
        absoluteLayout.AddView(entry, 400, 200, 150, 20);
        
        var entry2 = new EntryView(assets, assets.FontAtlas.DefaultFont, 18, .5f);
        entry2.BackgroundColor = Color.GOLD;
        entry2.RequestedSize = new Point(150, 20);
        entry2.Padding = new Thickness(5);
        entry2.Text = "World";
        absoluteLayout.AddView(entry2, 400, 400, 150, 20);

        _entityBrowserView = new EntityBrowserView(assets, _entityManager);
        _entityBrowserView.BackgroundColor = Color.GRAY;
        
        absoluteLayout.AddView(_entityBrowserView, windowData.Width - 300, 0, 300, windowData.Height);

        _uiRenderer = new UiRenderer(absoluteLayout, 0, 0, windowData.Width, windowData.Height);
    }

    private void SelectedShipFrame(Sprite selectedSprite)
    {
        if (_activeEntity != null)
        {
            _entityManager.DestroyEntity(_activeEntity.Value);
            _activeEntity = null;
        }

        var entity = _entityManager.CreateEntity();
        _entityManager.SetComponentFor(entity, new FollowMouseComponent());
        _entityManager.SetComponentFor(entity, new SpriteComponent(_spriteAtlas.AtlasName, selectedSprite.SpriteName, 0));
        _entityManager.SetComponentFor(entity, new TransformComponent());
        _activeEntity = entity;
        _entityBrowserView.Entity = entity;
    }

    public override void PerformTick(FrameTiming frameTiming)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            if (_activeEntity.HasValue && _entityManager.HasComponentFor<FollowMouseComponent>(_activeEntity.Value))
            {
                _entityManager.RemoveComponentFor<FollowMouseComponent>(_activeEntity.Value);
            }
        }
        
        _entityManager.RunSystems(frameTiming);
        RenderUi(in frameTiming);
    }

    public void RenderUi(in FrameTiming frameTiming)
    {
        _uiRenderer.Render(in frameTiming);
    }
}