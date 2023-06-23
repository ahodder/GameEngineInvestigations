using System.Text.Json.Serialization;

namespace Cyberstar.Sprites;

public class Sprite
{
    [JsonPropertyName("name")]
    public string SpriteName { get; set; }
    
    [JsonPropertyName("frames")]
    public List<SpriteFrame> Frames { get; set; }
}