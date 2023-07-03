using System.Numerics;

namespace Cyberstar.Game.Components;

public struct TransformComponent
{

    public Vector2 Position;

    public float Rotation;
    // public Matrix4x4 Transform;

    public void Translate(Vector2 movement)
    {
        Position += movement;
    }

    public void Rotate(float degrees)
    {
        // var radians = degrees * MathF.PI / 180;
        // var rotation = Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, radians);
        Rotation += degrees;
    }

    public static TransformComponent FromTranslation(Vector2 translation)
    {
        var ret = new TransformComponent();
        ret.Translate(translation);
        return ret;
    }
}