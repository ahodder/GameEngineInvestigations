using Cyberstar.UI;

namespace Cyberstar.AssetManagement;

public class AssetManager
{
    public FontAtlas FontAtlas { get; }

    public AssetManager()
    {
        FontAtlas = new FontAtlas("assets/fonts/NugoSansLight.ttf");
    }
}