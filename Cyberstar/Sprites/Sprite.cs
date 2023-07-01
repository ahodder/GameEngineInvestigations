using Newtonsoft.Json;

namespace Cyberstar.Sprites;

public class Sprite
{
    [JsonProperty("name")]
    public string SpriteName { get; set; }
    
    [JsonProperty("frames")]
    public List<SpriteFrame> Frames { get; set; }

    public bool Animated => Frames.Count > 1;
}