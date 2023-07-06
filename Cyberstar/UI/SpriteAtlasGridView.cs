using System.Drawing;
using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Extensions.Raylib;
using Cyberstar.Sprites;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Rectangle = Raylib_cs.Rectangle;

namespace Cyberstar.UI;

public class SpriteAtlasGridView : ViewBase
{
    public SpriteAtlas SpriteAtlas { get; set; }
    public int Columns { get; set; }
    public int Rows { get; set; }
    public int ColumnSpacing { get; set; }
    public int RowSpacing { get; set; }
    public Action<int, int>? OnCellClick { get; set; }
    public Func<int, int, bool>? OnHover = (x, y) => false;

    public int CellWidth => (ContentBounds.Width - ColumnSpacing * (Columns + 1)) / Columns;
    public int CellHeight => (ContentBounds.Height - RowSpacing * (Rows + 1)) / Rows;

    public SpriteAtlasGridView(AssetManager assetManager, SpriteAtlas spriteAtlas) : base(assetManager)
    {
        SpriteAtlas = spriteAtlas;
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        throw new NotImplementedException();
    }

    protected override void DoRenderContent()
    {
        var spriteAtlasKeys = SpriteAtlas.Sprites.Keys.ToArray();
        
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