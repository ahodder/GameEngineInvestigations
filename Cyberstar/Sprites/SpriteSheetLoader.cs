using System.Text.Json;
using Cyberstar.Components;
using Cyberstar.Core;

namespace Cyberstar.Sprites;

public class SpriteSheetLoader
{
    private const string FilePath = ".json";
    
    private string _basePath;

    public SpriteSheetLoader(string basePath)
    {
        var baseDir = Directory.GetCurrentDirectory();
        _basePath = Path.Join(baseDir, basePath);
    }
    
    public Result<SpriteSheetComponent> TryLoadSpriteSheet(string spriteSheet)
    {
        var path = Path.Join(_basePath, spriteSheet, FilePath);
        if (!Path.Exists(path))
        {
            return new Result<SpriteSheetComponent>("Failed to load sprite sheet: invalid path");
        }

        try
        {
            var contents = File.ReadAllText(path);
            var result = JsonSerializer.Deserialize<SpriteSheet>(contents);
            var val = new SpriteSheet();
            return default;
        }
        catch (Exception e)
        {
            return new Result<SpriteSheetComponent>("Failed  to load sprite: failed to load sprite sheet", e);
        }
    }
}