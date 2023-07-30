using Cyberstar.Engine.Logging;
using Cyberstar.Sprites;
using Newtonsoft.Json;
using Raylib_cs;

namespace Cyberstar.Engine.AssetManagement;

public class AssetManager
{
    private const string FontDirectory = "fonts";
    private const string SpriteAtlasDirectory = "textures/sprite_atlases";
    private const string TexturesDirectory = "textures";
    private const string TracksDirectory = "audio/tracks";
    private const string SoundByteDirectory = "audio/sound_bytes";
    
    public FontAtlas FontAtlas { get; }

    private readonly ILogger _logger;
    private string _rootDirectory;

    private readonly Dictionary<string, SpriteAtlas> _loadedAtlases = new();
    private readonly Dictionary<string, Music> _loadedMusic = new();


    public AssetManager(ILogger logger, string rootDirectory)
    {
        _logger = logger;
        _rootDirectory = rootDirectory;
        var fontPath = Path.Join(Directory.GetCurrentDirectory(), _rootDirectory, FontDirectory, "NugoSansLight.ttf");
        FontAtlas = new FontAtlas(fontPath);
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
            AtlasName = assetFileName,
            Sprites = sprites,
        };

        _loadedAtlases[assetFileName] = spriteAtlas;
        
        return texture.height != 0 && texture.width != 0;
    }

    public bool TryGetSpriteFromAtlas(string spriteAtlas, string spriteAnimation, int frameIndex, out Texture2D texture, out Rectangle frame)
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

    public bool TryGetSpriteAtlas(string spriteAtlasName, out SpriteAtlas spriteAtlas)
    {
        if (_loadedAtlases.TryGetValue(spriteAtlasName, out spriteAtlas))
            return true;

        spriteAtlas = default;
        return false;
    }

    public bool TryGetAudioTrack(string trackName, out Music music)
    {
        if (_loadedMusic.TryGetValue(trackName, out music))
            return true;
        
        var atlasDirectory = Path.Join(Directory.GetCurrentDirectory(), _rootDirectory, TracksDirectory);
        var audioPath = Path.Join(atlasDirectory, trackName + ".mp3");
        if (!Path.Exists(audioPath))
        {
            music = default;
            return false;
        }

        var stream = Raylib.LoadMusicStream(audioPath);
        music = stream;
        return true;
    }
}