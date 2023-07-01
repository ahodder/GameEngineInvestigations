using Cyberstar.Logging;
using Cyberstar.Sprites;
using Newtonsoft.Json;
using Raylib_cs;

namespace Cyberstar.AssetManagement;

public class AssetManager
{
    private const string FontDirectory = "fonts";
    private const string SpriteAtlasDirectory = "textures/sprite_atlases";
    private const string TexturesDirectory = "textures";
    
    public FontAtlas FontAtlas { get; }

    private readonly ILogger _logger;
    private string _rootDirectory;
    
    private Dictionary<string, SpriteAtlas> _loadedAtlases;


    public AssetManager(ILogger logger, string rootDirectory)
    {
        _logger = logger;
        _rootDirectory = rootDirectory;
        FontAtlas = new FontAtlas("assets/fonts/NugoSansLight.ttf");
        _loadedAtlases = new Dictionary<string, SpriteAtlas>();
    }

    public bool TryLoadSpriteAtlas(string assetFileName, out SpriteAtlas? spriteAtlas)
    {
        if (_loadedAtlases.TryGetValue(assetFileName, out var atlas))
        {
            spriteAtlas = atlas;
            return true;
        }

        var atlasDirectory = Path.Join(Directory.GetCurrentDirectory(), _rootDirectory, SpriteAtlasDirectory);
        var jsonPath = Path.Join(atlasDirectory, assetFileName + ".json");
        var texturePath = Path.Join(atlasDirectory, assetFileName + ".png");

        if (!Path.Exists(jsonPath))
        {
            _logger.Error($"Failed to load texture: json path {jsonPath} does not exist");
            spriteAtlas = default;
            return false;
        }
        
        if (!Path.Exists(texturePath))
        {
            _logger.Error($"Failed to load texture: texture path {texturePath} does not exist");
            spriteAtlas = default;
            return false;
        }

        var json = File.ReadAllText(jsonPath);
        var sheet = JsonConvert.DeserializeObject<JsonSpriteSheet>(json);

        if (sheet == null)
        {
            _logger.Error($"Failed to deserialize the sprite sheet");
            spriteAtlas = default;
            return false;
        }
        
        var sprites = new Dictionary<string, Sprite>();
        for (var i = 0; i < sheet.Sprites.Count; i++)
        {
            var sprite = sheet.Sprites[i];
            sprites[sprite.SpriteName] = sprite;
        }

        var texture = Raylib.LoadTexture(texturePath);
        spriteAtlas = new SpriteAtlas
        {
            BackingTexture = texture,
            Sprites = sprites,
        };

        _loadedAtlases[assetFileName] = spriteAtlas;
        
        return texture.height != 0 && texture.width != 0;
    }

    public bool GetSpriteFromAtlas(string spriteAtlas, string spriteAnimation, int frameIndex, out Texture2D texture, out Rectangle frame)
    {
        if (!_loadedAtlases.TryGetValue(spriteAtlas, out var atlas) ||
            !atlas.Sprites.TryGetValue(spriteAnimation, out var sprite) ||
            sprite.Frames.Count < frameIndex)
        {
            texture = default;
            frame = default;
            return false;
        }

        var spriteFrame = sprite.Frames[frameIndex];
        texture = atlas.BackingTexture;
        frame = new Rectangle(spriteFrame.X, spriteFrame.Y, spriteFrame.Width, spriteFrame.Height);
        return true;
    }
}