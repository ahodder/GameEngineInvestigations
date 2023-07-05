using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Extensions.Raylib;
using Cyberstar.Sprites;
using Cyberstar.UI.ViewFragments;
using Raylib_cs;

namespace Cyberstar.UI;

public struct SpriteAtlasGridView : IView
{
    public ViewDimensions Dimensions { get; }
    public BackgroundFragment Background;
    public SpriteAtlas SpriteAtlas;
    public int Columns;
    public int Rows;
    public int ColumnSpacing;
    public int RowSpacing;
    public Action<int, int>? OnCellClick;
    public Func<int, int, bool>? OnHover = (x, y) => false;

    public int CellWidth => (Dimensions.ContentWidth - ColumnSpacing * (Columns + 1)) / Columns;
    public int CellHeight => (Dimensions.ContentHeight - RowSpacing * (Rows + 1)) / Rows;

    public SpriteAtlasGridView()
    {
    }

    public void Render(AssetManager assetManager)
    {
        Background.Render(assetManager, Dimensions);

        var spriteAtlasKeys = SpriteAtlas.Sprites.Keys.ToArray();
        Raylib.DrawRectangle(Dimensions.ActualX, Dimensions.ActualY, Dimensions.ActualWidth, Dimensions.ActualHeight, Color.GOLD);
        
        var mousePosition = Raylib.GetMousePosition();
        
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                var w = CellWidth;
                var h = CellHeight;

                var size = MathF.Min(w, h);
                
                var cOffset = c * (w + ColumnSpacing) + ColumnSpacing;
                var rOffset = r * (h + RowSpacing) + RowSpacing;
                
                Raylib.DrawRectangle(cOffset, rOffset, CellWidth, CellHeight, Color.BLACK);
                
                var index = Columns * r + c;
                if (index >= spriteAtlasKeys.Length) continue;
                var sprite = SpriteAtlas.Sprites[spriteAtlasKeys[index]];
                var frame = sprite.Frames[0];

                var wo = (w - size) / 2;
                var ho = (h - size) / 2;
                var destBounds = new Rectangle(cOffset + wo, rOffset + ho, size, size);

                if (destBounds.Contains(mousePosition))
                {
                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                        OnCellClick?.Invoke(c, r);
                    
                    if (OnHover?.Invoke(c, r) ?? false)
                        Raylib.DrawRectangle(cOffset, rOffset, CellWidth, CellHeight, Color.GREEN);
                }
                
                Raylib.DrawTexturePro(SpriteAtlas.BackingTexture,
                    new Rectangle(frame.X, frame.Y, frame.Width, frame.Height),
                    destBounds,
                    Vector2.Zero,
                    0, Color.WHITE);
            }
        }
    }
}