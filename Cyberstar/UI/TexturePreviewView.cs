using System.Drawing;
using System.Numerics;
using Cyberstar.AssetManagement;
using Cyberstar.Core;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Rectangle = Raylib_cs.Rectangle;

namespace Cyberstar.UI;

public class TexturePreviewView : ViewBase
{
    public Texture2D Texture { get; set; }
    public Rectangle TextureContents { get; set; }
    
    public TexturePreviewView(AssetManager assetManager) : base(assetManager)
    {
    }

    public TexturePreviewView(AssetManager assetManager, 
        Texture2D texture, 
        Rectangle textureContents,
        int width, int height) : base(assetManager)
    {
        Texture = texture;
        TextureContents = textureContents;
        RequestedSize = new Point(width, height);
    }

    protected override Point DoMeasure(int x, int y, int width, int height)
    {
        if (RequestedSize != Point.Empty)
            return RequestedSize;
        else
            return new Point(width, height);

    }

    protected override void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        Raylib.DrawTexturePro(Texture,
            TextureContents,
            new Rectangle(ContentBounds.X, ContentBounds.Y, ContentBounds.Width, ContentBounds.Height),
            Vector2.Zero,
            0,
            Color.WHITE);
    }
}