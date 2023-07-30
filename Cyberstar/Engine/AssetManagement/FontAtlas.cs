using Raylib_cs;

namespace Cyberstar.Engine.AssetManagement;

public readonly struct FontId
{
    public readonly uint Id;

    public FontId(uint id)
    {
        Id = id;
    }
}

public class FontAtlas
{
    public Font DefaultFont { get; }
    
    private Dictionary<string, FontId> _pathToFontMapping;
    private Dictionary<FontId, Font> _fontMapping;
    private uint _count;

    public FontAtlas(string defaultFontPath)
    {
        DefaultFont = Raylib.LoadFont(defaultFontPath);
        _pathToFontMapping = new Dictionary<string, FontId>();
        _fontMapping = new Dictionary<FontId, Font>();
    }

    public bool TryLoadFont(string fontPath, out FontId fontId)
    {
        if (_pathToFontMapping.TryGetValue(fontPath, out fontId))
            return true;
        
        /* todo ahodder@quantum-intl.com 6/28/23: We need to log this somehow */
        try
        {
            var font = Raylib.LoadFont(fontPath);
            fontId = new FontId(++_count);
            _pathToFontMapping[fontPath] = fontId;
            _fontMapping[fontId] = font;
            return true;
        }
        catch
        {
            fontId = default;
            return false;
        }
    }

    public bool TryGetFont(FontId fontId, out Font font)
    {
        if (_fontMapping.TryGetValue(fontId, out font))
            return true;

        font = default;
        return false;
    }
}