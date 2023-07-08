using System.Drawing;
using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Extensions.Raylib;
using Cyberstar.Sprites;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Rectangle = Raylib_cs.Rectangle;

namespace Cyberstar.UI;

public delegate void SpriteClicked(Sprite selectedSprite);

public class SpriteAtlasGridView : ViewBase
{
    public SpriteAtlas SpriteAtlas { get; set; }
    public int Columns { get; set; }
    public int Rows { get; set; }
    public int ColumnSpacing { get; set; }
    public int RowSpacing { get; set; }
    public SpriteClicked? OnCellClick { get; set; }
    public Func<int, int, bool>? OnHover = (x, y) => false;

    public int CellWidth { get; set; }
    public int CellHeight { get; set; }

    public SpriteAtlasGridView(AssetManager assetManager, SpriteAtlas spriteAtlas) : base(assetManager)
    {
        SpriteAtlas = spriteAtlas;
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        var w = Columns * (ColumnSpacing * 2 + CellWidth);
        var h = Rows * (RowSpacing * 2 + CellHeight);
        return new Point(w, h);
    }

    protected override void DoRenderContent(in InputData inputData)
    {
        var spriteAtlasKeys = SpriteAtlas.Sprites.Keys.ToArray();
        
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                var w = CellWidth;
                var h = CellHeight;

                var size = MathF.Min(w, h);
                
                var cOffset = ContentBounds.X + c * (w + ColumnSpacing) + ColumnSpacing;
                var rOffset = ContentBounds.Y + r * (h + RowSpacing) + RowSpacing;
                
                Raylib.DrawRectangle(cOffset, rOffset, CellWidth, CellHeight, Color.BLACK);
                
                var index = Columns * r + c;
                if (index >= spriteAtlasKeys.Length) continue;
                var sprite = SpriteAtlas.Sprites[spriteAtlasKeys[index]];
                var frame = sprite.Frames[0];

                var wo = (w - size) / 2;
                var ho = (h - size) / 2;
                var destBounds = new Rectangle(cOffset + wo, rOffset + ho, size, size);
                
                if (destBounds.Contains(new Vector2(inputData.MouseX, inputData.MouseY)))
                {
                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                        OnCellClick?.Invoke(sprite);

                    if (OnHover?.Invoke(inputData.MouseX, inputData.MouseY) ?? false)
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