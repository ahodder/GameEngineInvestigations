using Cyberstar.Core;
using Cyberstar.Engine;
using Cyberstar.Engine.ECS;
using Cyberstar.Engine.Scenes;
using Cyberstar.Engine.UI;
using Cyberstar.Game.Components;
using Cyberstar.Game.Systems;
using Cyberstar.Sprites;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Cyberstar.Game.Scenes;

public class ShipBuilderScene : Scene
{
    private SpriteAtlas _spriteAtlas;

    private TexturePreviewView _selectedFramePreview;
    private EntityBrowserView _entityBrowserView;
    private UiRenderer _uiRenderer;
    private EntityManager _entityManager;

    private Entity? _activeEntity;

    public ShipBuilderScene(IEngine engine) : base(engine)
    {
        _entityManager = new EntityManager(engine.Logger, 16);
        _entityManager.AddSystem(new TextureFollowMouseSystem());
        _entityManager.AddSystem(new SpriteRenderingSystem(engine.AssetManager));
        _entityManager.AddSystem(new EntityOriginRendererSystem());
        
        Engine.AssetManager.TryLoadSpriteAtlas("dev_ships", out _spriteAtlas);
        var atlasView = new SpriteAtlasGridView(Engine.AssetManager, _spriteAtlas);
        atlasView.ColumnSpacing = 10;
        atlasView.RowSpacing = 10;
        atlasView.CellWidth = 50;
        atlasView.CellHeight = 50;
        atlasView.Columns = 4;
        atlasView.Rows = 4;
        atlasView.OnCellClick = SelectedShipFrame;
        atlasView.OnHover = (x, y) => true;

        var spriteCollection = new LabeledExpanderView(Engine.AssetManager, atlasView, "Ship Frames");
        spriteCollection.Header.OnClick = () => spriteCollection.IsExpanded = !spriteCollection.IsExpanded;

        var absoluteLayout = new AbsoluteLayout(Engine.AssetManager);
        absoluteLayout.AddView(spriteCollection, 0, 0, 400, Engine.WindowData.Height);

        _entityBrowserView = new EntityBrowserView(Engine.AssetManager, _entityManager);
        _entityBrowserView.BackgroundColor = Color.GRAY;
        _entityBrowserView.HorizontalMeasure = EAxisMeasure.FillParent;
        
        absoluteLayout.AddView(_entityBrowserView, engine.WindowData.Width - 300, 0, 0, engine.WindowData.Height);

        _uiRenderer = new UiRenderer(absoluteLayout, 0, 0, engine.WindowData.Width, engine.WindowData.Height);
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