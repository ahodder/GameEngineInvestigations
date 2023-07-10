using System.Drawing;
using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Cyberstar.Extensions.Raylib;
using Cyberstar.Sprites;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Rectangle = Raylib_cs.Rectangle;

namespace Cyberstar.UI;

public delegate void SpriteClicked(Sprite selectedSprite);

public class SpriteAtlasGridView : ViewBase
{
    public SpriteAtlas SpriteAtlas { get; }
    public string[] Keys { get; }
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
        Keys = SpriteAtlas.Sprites.Keys.ToArray();
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        var w = Columns * (ColumnSpacing * 2 + CellWidth);
        var h = Rows * (RowSpacing * 2 + CellHeight);
        return new Point(w, h);
    }

    protected override void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
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
                if (index >= Keys.Length) continue;
                var sprite = SpriteAtlas.Sprites[Keys[index]];
                var frame = sprite.Frames[0];

                var wo = (w - size) / 2;
                var ho = (h - size) / 2;

                var widthAspectRatio = frame.Width / (float)frame.Height;
                var heightAspectRatio = frame.Height / (float)frame.Width;

                var widthSize = size;
                var heightSize = size;
                if (widthAspectRatio < heightAspectRatio)
                    widthSize *= widthAspectRatio;
                else
                    heightSize *= heightAspectRatio;

                var destBounds = new Rectangle(cOffset + wo + (size - widthSize) / 2, rOffset + ho + (size - heightSize) / 2, widthSize, heightSize);
                
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

    public override bool WillHandleMouseClick(int mouseX, int mouseY)
    {
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                var w = CellWidth;
                var h = CellHeight;

                var size = MathF.Min(w, h);
                
                var cOffset = ContentBounds.X + c * (w + ColumnSpacing) + ColumnSpacing;
                var rOffset = ContentBounds.Y + r * (h + RowSpacing) + RowSpacing;
                
                var index = Columns * r + c;
                if (index >= Keys.Length) continue;

                var wo = (w - size) / 2;
                var ho = (h - size) / 2;

                var destBounds = new Rectangle(cOffset + wo, rOffset + ho, size, size);
                
                if (destBounds.Contains(new Vector2(mouseX, mouseY)))
                {
                    var sprite = SpriteAtlas.Sprites[Keys[index]];
                    OnCellClick?.Invoke(sprite);
                    return true;
                }
            }
        }

        return false;
    }
}