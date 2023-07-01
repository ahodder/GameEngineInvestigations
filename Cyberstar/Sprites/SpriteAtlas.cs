using Newtonsoft.Json;
using Raylib_cs;

namespace Cyberstar.Sprites;

public class SpriteAtlas
{
    public Texture2D BackingTexture;

    [JsonProperty("sprites")]
    public Dictionary<string, Sprite> Sprites;
    
    
}