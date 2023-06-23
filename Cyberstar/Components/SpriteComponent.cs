using Raylib_cs;

namespace Cyberstar.Components;

public struct SpriteComponent
{
    public string Name;
    public Frame[] Frames;

    public SpriteComponent(string name, Frame[] frames)
    {
        Name = name;
        Frames = frames;
    }
    
    public readonly struct Frame
    {
        public readonly Rectangle Rect;

        public Frame(Rectangle rect)
        {
            Rect = rect;
        }
    }
}