using Newtonsoft.Json;
using Raylib_cs;

namespace Cyberstar.Sprites;

public class SpriteAtlas
{
    public Texture2D BackingTexture;

    public string AtlasName { get; set; }

    [JsonProperty("sprites")]
    public Dictionary<string, Sprite> Sprites { get; set; }
}