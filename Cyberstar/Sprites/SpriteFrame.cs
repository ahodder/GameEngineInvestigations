using System.Text.Json.Serialization;
using Raylib_cs;

namespace Cyberstar.Sprites;

public class SpriteFrame
{
    [JsonPropertyName("x")]
    public int X { get; set; }
    
    [JsonPropertyName("y")]
    public int Y { get; set; }
    
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonIgnore]
    public Rectangle Rectangle => new Rectangle(X, Y, Width, Height);
}