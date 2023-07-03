using System.Numerics;
using Cyberstar.Math;

namespace Cyberstar.Game.Components;

public struct TransformComponent
{
    public Vector2 Position { get; private set; }
    public Vector2 Scale { get; private set; }
    public float RotationRadians { get; private set; }

    public Vector2 Forward => new Vector2(MathF.Cos(RotationRadians), MathF.Sin(RotationRadians));

    public void Translate(Vector2 movement)
    {
        Position += movement;
    }

    public void TranslateForward(Vector2 movement)
    {
        Position += movement.Rotate(RotationRadians);
    }

    public void Rotate(float degrees)
    {
        RotationRadians += degrees * MathF.PI / 180;
    }

    public static TransformComponent FromTranslation(Vector2 translation)
    {
        var ret = new TransformComponent();
        ret.Translate(translation);
        return ret;
    }
}


/*

using System.Numerics;

namespace Cyberstar.Game.Components;

public struct TransformComponent
{

    public Vector2 Position;
    public Vector2 Scale;
    public float RotationRadians; // This is in radians

    public float RotationDegrees
    {
        get => RotationRadians * 180 / MathF.PI;
        set => RotationRadians = value * 180 / MathF.PI;
    }

    // Calculated on-the-fly
    public Vector2 Forward => new Vector2(MathF.Cos(RotationRadians), MathF.Sin(RotationRadians));

    // public Matrix4x4 Transform;

    public void Translate(Vector2 movement)
    {
        Position += movement;
    }

    public void TranslateForward(Vector2 movement)
    {
        Position += new Vector2(movement.X * MathF.Cos(RotationRadians), movement.Y * MathF.Sin(RotationRadians));
    }

    public static TransformComponent FromTranslation(Vector2 translation)
    {
        var ret = new TransformComponent();
        ret.Translate(translation);
        return ret;
    }
}

*/