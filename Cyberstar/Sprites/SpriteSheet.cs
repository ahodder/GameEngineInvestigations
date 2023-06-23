using System.Text.Json.Serialization;

namespace Cyberstar.Sprites;

public class SpriteSheet
{
    [JsonPropertyName("spites")]
    public List<Sprite> Sprites { get; set; }
}