using Raylib_cs;
using Cyberstar.Strings;

namespace Cyberstar.Engine.AssetManagement;

public struct AnimationCollection
{
    public List<ModelAnimation> Animations { get; }

    public AnimationCollection(ReadOnlySpan<ModelAnimation> animations)
    {
        Animations = new List<ModelAnimation>();
        for (var i = 0; i < animations.Length; i++)
            Animations.Add(animations[i]);
    }
}