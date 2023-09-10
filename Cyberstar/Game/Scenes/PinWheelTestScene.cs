using System.Numerics;
using Cyberstar.Core;
using Cyberstar.Engine;
using Cyberstar.Engine.Scenes;
using Raylib_cs;

namespace Cyberstar.Game.Scenes;

public class PinWheelTestScene : Scene
{
    private Armature _armature;
    private PinWheel _wheel;
    
    public PinWheelTestScene(IEngine engine) : base(engine)
    {
        var radius = Engine.WindowData.Height / 2;
        _wheel = new PinWheel
        {
            center = new Vector3(engine.WindowData.Width / 2f, engine.WindowData.Height / 2f, 0),
            armRadius = radius,
        };

        _armature = new Armature
        {
            origin = new Vector3(Engine.WindowData.Width / 2f, Engine.WindowData.Height - 90f, 0),
            bones = new[]
            {
                new Bone(50f, 0f),
                new Bone(40f, 0),
            }
        };
    }

    public override void PerformTick(FrameTiming frameTiming)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            _wheel.velocity -= PinWheel.Acceleration;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            _wheel.velocity += PinWheel.Acceleration;
        
        if (_wheel.armRadius < 0.01f) _wheel.armRadius = 0.01f;
        _wheel.floor = Engine.WindowData.Height;
        _wheel.Update(frameTiming);
        _wheel.center.Y = _wheel.armRadius;
    }

    private struct Armature
    {
        public Vector3 origin;
        public Bone[] bones;

        /// <summary>
        /// FABRIKIK (Forward and Backward Reaching Inverse Kinematics) is used to attempt
        /// to align an armature towards some target point.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="tolerance"></param>
        public void PerformFabrikik(Vector2 target, float tolerance)
        {
            
        }
    }

    private struct Bone
    {
        public float boneLength;
        public float rotation;

        public Bone(float boneLength, float rotation)
        {
            this.boneLength = boneLength;
            this.rotation = rotation;
        }
    }

    private struct PinWheel
    {
        public const float CircleTime = 2 * MathF.PI;
        public const float QuarterCircle = CircleTime / 4f;
        public const float Acceleration = 1f;

        public float velocity;
        public float armRadius;
        public Vector3 center;
        public float rotationRadians;
        public float floor;
        
        public void Update(FrameTiming frameTiming)
        {
            if (velocity == 0) return;
            armRadius = Math.Abs(velocity / CircleTime);
            center.X += velocity * frameTiming.DeltaTime;
            rotationRadians -= (velocity * frameTiming.DeltaTime) / armRadius;
            for (var i = 0; i < 4; i++)
            {
                var color = i == 0 ? Color.WHITE : Color.RED;
                var rads = rotationRadians + QuarterCircle * i;
                // Draw the first arm
                var px = center.X + MathF.Cos(rads) * armRadius;
                var py = center.Y + MathF.Sin(rads) * armRadius;
                Raylib.DrawLine((int)center.X, (int)(floor - center.Y), (int)px, (int)(floor - py), color);
            }
            
            Raylib.DrawLine((int)center.X, (int)(floor - center.Y), (int)center.X, (int)floor, Color.BLUE);
        }
    }
}
