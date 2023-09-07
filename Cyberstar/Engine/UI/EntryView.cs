using System.Drawing;
using System.Numerics;
using Cyberstar.Core;
using Cyberstar.Engine.AssetManagement;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Cyberstar.Engine.UI;

public class EntryView : ViewBase
{
    private const float CaretWidth = 1;
    private const float KeyRepeatDelay = .1f;
    
    public ReadOnlySpan<char> Text
    {
        get => _charBuffer.AsSpan(_charCount);
        set
        {
            value.CopyTo(_charBuffer);
            _charCount = value.Length;
            _charBuffer[value.Length] = '\0';
        }
    }
    
    public Font Font { get; set; }
    public int FontSize { get; set; }
    public float FontSpacing { get; set; }
    public Color TextColor { get; set; }
    public Color CaretColor { get; set; }
    public float CaretFlashSpeedSeconds { get; set; }
    /// <summary>
    /// Which character the insertion point is in front of.
    /// </summary>
    public int InsertionPointPosition { get; set; }

    private bool _showCaret;
    private float _timeSinceLastCaretFlash;
    private char[] _charBuffer;
    private int _charCount;
    private KeyboardKey _lastPressedKey;
    private float _timeSinceLastKeyRepeat;
    private float _keyRepeatDelay;
    
    public EntryView(AssetManager assetManager,
        Font? font = null,
        int fontSize = 18,
        float fontSpacing = 1f,
        float caretFlashSpeedSeconds = 0.5f) : base(assetManager)
    {
        Font = font ?? assetManager.FontAtlas.DefaultFont;
        FontSize = fontSize;
        FontSpacing = fontSpacing;
        CaretFlashSpeedSeconds = caretFlashSpeedSeconds;
        _charBuffer = new char[64];
        CaretColor = Color.BLACK;
        _keyRepeatDelay = KeyRepeatDelay;
        
        TextColor = Color.WHITE;
    } 

    protected override unsafe Point MeasureSelf(int x, int y, int width, int height)
    {
        var bytes = stackalloc sbyte[_charCount];
        for (var i = 0; i < _charCount; i++)
            bytes[i] = (sbyte)_charBuffer[i];
        var vec = Raylib.MeasureTextEx(Font, bytes, FontSize, FontSpacing);
        // So, if the test is null or empty, we will have a height of 0. Let's just set the height to the font size
        vec.Y = FontSize;
        return new Point((int)vec.X, (int)vec.Y);
    }

    protected override unsafe void DoRenderContent(in FrameTiming frameTiming, in InputData inputData)
    {
        _timeSinceLastCaretFlash += frameTiming.DeltaTime;
        if (_timeSinceLastCaretFlash > CaretFlashSpeedSeconds)
        {
            _showCaret = !_showCaret;
            _timeSinceLastCaretFlash = 0;
        }
        
        var bytes = stackalloc sbyte[_charBuffer.Length];
        
        for (var i = 0; i < InsertionPointPosition; i++)
            bytes[i] = (sbyte)_charBuffer[i];
        var vec = Raylib.MeasureTextEx(Font, bytes, FontSize, FontSpacing);
        
        if (_showCaret && HasFocus)
            Raylib.DrawRectangle(ContentBounds.X  + (int)vec.X, ContentBounds.Y + 0, (int)CaretWidth, FontSize, CaretColor);
        
        for (var i = InsertionPointPosition; i < _charBuffer.Length; i++)
            bytes[i] = (sbyte)_charBuffer[i];
        
        vec = Raylib.MeasureTextEx(Font, bytes, FontSize, FontSpacing);
        
        Raylib.DrawRectangleLines(ContentBounds.X, ContentBounds.Y, (int)vec.X, (int)vec.Y, Color.GREEN);
        if (_charCount > 0)
            Raylib.DrawTextEx(Font, bytes, new Vector2(ContentBounds.X, ContentBounds.Y), FontSize, FontSpacing, TextColor);
    }
    
    public override void HandleKeyboardKeys(in FrameTiming frameTiming, Span<KeyboardKey> keys)
    {
        if (!HasFocus) return;
        
        for (var i = 0; i < keys.Length; i++)
        {
            var key = keys[i];

            if (key >= KeyboardKey.KEY_ZERO && key <= KeyboardKey.KEY_NINE)
            {
                var c = (char)('0' + key - KeyboardKey.KEY_ZERO);
                InsertChar(c);
            }
            
            else if (key >= KeyboardKey.KEY_A && key <= KeyboardKey.KEY_Z)
            {
                var c = (char)('A' + key - KeyboardKey.KEY_A);
                c = Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SHIFT) ? c : (char)(c + 32);
                InsertChar(c);
            }
            
            switch (key)
            {
                case KeyboardKey.KEY_LEFT:
                    InsertionPointPosition = Math.Max(InsertionPointPosition - 1, 0);
                    break;
                
                case KeyboardKey.KEY_RIGHT:
                    InsertionPointPosition = Math.Min(InsertionPointPosition + 1, _charCount);
                    break;
                
                case KeyboardKey.KEY_SPACE:
                    InsertChar(' ');
                    break;
            }
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.KEY_BACKSPACE) && _timeSinceLastKeyRepeat > _keyRepeatDelay)
        {
            _timeSinceLastKeyRepeat = 0;
            if (InsertionPointPosition > 0)
            {
                for (var j = InsertionPointPosition - 1; j < _charCount; j++)
                    _charBuffer[j] = _charBuffer[j + 1];
                _charBuffer[_charCount] = '\0';
                InsertionPointPosition--;
                _charCount--;
            }
        }

        _timeSinceLastKeyRepeat += frameTiming.DeltaTime;
    }

    public override bool WillHandleMouseClick(int mouseX, int mouseY)
    {
        if (PaddedBounds.Contains(mouseX, mouseY))
        {
            HasFocus = true;
            return true;
        }

        HasFocus = false;
        return false;
    }

    public void SetText(ReadOnlySpan<char> text)
    {
        EnsureCapacity(text.Length + 1);
        text.CopyTo(_charBuffer);
        _charBuffer[text.Length] = '\0';
        _charCount = text.Length;
    }

    public void InsertChar(char insertedChar)
    {
        EnsureCapacity(_charCount + 2);
        _charBuffer[_charCount + 2] = '\0';

        var cap = Math.Max(InsertionPointPosition, 1);
        for (var i = _charCount + 1; i >= cap; i--)
        {
            _charBuffer[i] = _charBuffer[i - 1];
        }

        _charBuffer[InsertionPointPosition] = insertedChar;
        InsertionPointPosition++;
        _charCount++;
    }

    public void EnsureCapacity(int count)
    {
        if (count > _charBuffer.Length)
        {
            var tmp = new char[_charBuffer.Length * 2];
            Array.Copy(_charBuffer, 0, tmp, 0, _charBuffer.Length);
            _charBuffer = tmp;
        }
    }
}