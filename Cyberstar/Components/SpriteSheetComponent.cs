using Raylib_cs;

namespace Cyberstar.Components;

public struct SpriteSheetComponent
{
    public Texture2D SourceTexture;

    public SpriteSheetComponent(Texture2D texture)
    {
        SourceTexture = texture;
    }
}