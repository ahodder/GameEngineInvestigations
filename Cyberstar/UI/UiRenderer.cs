using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.UI.ViewFragments;
using Raylib_cs;

namespace Cyberstar.UI;

public class UiRenderer
{
    private readonly AssetManager _assetManager;
    
    public UiRenderer(AssetManager assetManager)
    {
        _assetManager = assetManager;
    }

    /// <summary>
    /// Draws a button to the screen.
    /// </summary>
    /// <param name="button"></param>
    public void DrawButton(in Button button)
    {
        RenderBackgroundFragment(button.Dimensions, button.Background);
        RenderTextFragment(button.Dimensions, button.Text);

        var mousePosition = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && button.Dimensions.Contains(mousePosition))
        {
            button.OnClick?.Invoke();
        }
    }

    /// <summary>
    /// Draws a label to the screen.
    /// </summary>
    /// <param name="label"></param>
    public void DrawLabel(in Label label)
    {
        RenderTextFragment(label.Dimensions, label.Text);
    }

    public Color[] Colors = new[] { Color.RED, Color.BLUE, Color.GREEN, Color.GOLD, Color.PURPLE };

    /// <summary>
    /// Draws a sprite gridview.
    /// </summary>
    /// <param name="spriteAtlasGridView"></param>
    public void DrawSpriteGridView(in SpriteAtlasGridView spriteAtlasGridView)
    {
        RenderBackgroundFragment(spriteAtlasGridView.Dimensions, spriteAtlasGridView.Background);

        var spriteAtlasKeys = spriteAtlasGridView.SpriteAtlas.Sprites.Keys.ToArray();
        var d = spriteAtlasGridView.Dimensions;
        Raylib.DrawRectangle(d.ActualX, d.ActualY, d.ActualWidth, d.ActualHeight, Color.GOLD);
        
        for (var r = 0; r < spriteAtlasGridView.Rows; r++)
        {
            for (var c = 0; c < spriteAtlasGridView.Columns; c++)
            {
                var w = spriteAtlasGridView.CellWidth;
                var h = spriteAtlasGridView.CellHeight;

                var size = MathF.Min(w, h);
                
                var cOffset = c * (w + spriteAtlasGridView.ColumnSpacing) + spriteAtlasGridView.ColumnSpacing;
                var rOffset = r * (h + spriteAtlasGridView.RowSpacing) + spriteAtlasGridView.RowSpacing;
                
                Raylib.DrawRectangle(cOffset, rOffset, spriteAtlasGridView.CellWidth, spriteAtlasGridView.CellHeight, Color.BLACK);
                
                var index = spriteAtlasGridView.Columns * r + c;
                if (index >= spriteAtlasKeys.Length) continue;
                var sprite = spriteAtlasGridView.SpriteAtlas.Sprites[spriteAtlasKeys[index]];
                var frame = sprite.Frames[0];

                var wo = (w - size) / 2;
                var ho = (h - size) / 2;

                Raylib.DrawTexturePro(spriteAtlasGridView.SpriteAtlas.BackingTexture,
                    new Rectangle(frame.X, frame.Y, frame.Width, frame.Height),
                    new Rectangle(cOffset + wo, rOffset + ho, size, size),
                    Vector2.Zero,
                    0, Color.WHITE);
            }
        }
    }

    /// <summary>
    /// Renders some text to the given dimensions.
    /// </summary>
    /// <param name="dimensionsFragment"></param>
    /// <param name="textFragment"></param>
    private void RenderTextFragment(in DimensionsFragment dimensionsFragment, in TextFragment textFragment)
    {
        var pos = new Vector2(dimensionsFragment.ContentStartX, dimensionsFragment.ContentStartY);

        Raylib.DrawTextEx(textFragment.Font, textFragment.Text, pos, textFragment.FontSize, 1, textFragment.TextColor);
    }

    /// <summary>
    /// Renders some background fragment.
    /// </summary>
    /// <param name="dimensionsFragment"></param>
    /// <param name="backgroundFragment"></param>
    private void RenderBackgroundFragment(in DimensionsFragment dimensionsFragment, in ViewBackgroundFragment backgroundFragment)
    {
        var x = dimensionsFragment.PaddedStartX;
        var y = dimensionsFragment.PaddedStartY;
        var w = dimensionsFragment.PaddedWidth;
        var h = dimensionsFragment.PaddedHeight;
        
        Raylib.DrawRectangle(x, y, w, h, backgroundFragment.BackgroundColor);
        
        Raylib.DrawRectangleLines(dimensionsFragment.ActualX, 
            dimensionsFragment.ActualY,
            dimensionsFragment.ActualWidth,
            dimensionsFragment.ActualHeight,
            backgroundFragment.BackgroundColor);
    }
}