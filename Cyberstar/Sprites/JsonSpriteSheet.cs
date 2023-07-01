
using Newtonsoft.Json;

namespace Cyberstar.Sprites;

public class JsonSpriteSheet
{
    [JsonProperty("sprites")]
    public List<Sprite> Sprites { get; set; }
}