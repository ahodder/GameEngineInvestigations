using System.Numerics;
using Cyberstar.Core;
using Cyberstar.Engine;
using Cyberstar.Engine.Scenes;
using Raylib_cs;

namespace Cyberstar.Game.Scenes;

public class PinWheelTestScene : Scene
{
    private Camera3D _camera;
    private Armature _armature;
    private PinWheel _wheel;
    private Ground _ground;
    private SpringCapsule _springCapsule;
    
    public PinWheelTestScene(IEngine engine) : base(engine)
    {
        var radius = Engine.WindowData.Height / 2;
        _wheel = new PinWheel
        {
            center = new Vector3(engine.WindowData.Width / 2f, engine.WindowData.Height / 2f, 0),
            armRadius = radius,
        };

        _armature = new Armature(
            new Vector3(Engine.WindowData.Width / 2f, Engine.WindowData.Height / 2f + 90f, 0),
            new[]
            {
                new Bone(40f),
                new Bone(50f),
                new Bone(70f),
                new Bone(80f),
            });

        _ground = new Ground();
        _springCapsule = new SpringCapsule(new Vector3(0, 2, 0), 1f);
        
        _camera = new Camera3D(new Vector3(0, 10, -15), Vector3.Zero, Vector3.UnitY, 90, CameraProjection.CAMERA_PERSPECTIVE);
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
        
        _armature.Update(frameTiming);
        
        _ground.Update(frameTiming, _camera);
        _springCapsule.Update(frameTiming, _camera);
    }

    private struct Ground
    {
        public void Update(FrameTiming frameTiming, Camera3D camera)
        {
            Raylib.BeginMode3D(camera);
            Raylib.DrawPlane(Vector3.Zero, new Vector2(10, 10), Color.GRAY);
            Raylib.EndMode3D();
        }
    }

    private struct SpringCapsule
    {
        private Vector3 _groundBumperOrigin;
        private float _groundBumperRadius;

        public SpringCapsule(Vector3 groundBumperOrigin, float groundBumperRadius)
        {
            _groundBumperOrigin = groundBumperOrigin;
            _groundBumperRadius = groundBumperRadius;
        }

        public void Update(FrameTiming frameTiming, Camera3D camera)
        {
            Raylib.BeginMode3D(camera);
            Raylib.DrawSphere(_groundBumperOrigin, 0.25f, Color.GOLD);
            Raylib.DrawSphereWires(_groundBumperOrigin, _groundBumperRadius, 5, 5, Color.GREEN);            
            Raylib.EndMode3D();
        }
    }
    
    private struct Armature
    {
        private const int NodeRadius = 7;
        public Vector3 Origin { get; }
        public Bone[] Bones { get; }

        public float MaxLength { get; }

        public Armature(Vector3 origin, Bone[] bones)
        {
            Origin = origin;
            Bones = bones;

            var maxLen = 0f;
            foreach (var bone in bones)
                maxLen += bone.BoneLength;
            MaxLength = maxLen;

            var o = Origin;
            Bones[0].Origin = Origin;
            for (var i = 1; i < Bones.Length; i++)
            {
                o = Vector3.UnitX * Bones[i - 1].BoneLength + o;
                Bones[i].Origin = o;
            }
        }

        public void Update(FrameTiming frameTiming)
        {
            var mx = Raylib.GetMouseX();
            var my = Raylib.GetMouseY();
            
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
                ForwardAndBackwardReachingInverseKinematics(new Vector3(mx, my, 0));

            for (var i = 0; i < Bones.Length - 1; i++)
            {
                var o1 = Bones[i].Origin;
                var o2 = Bones[i + 1].Origin;
                Raylib.DrawCircleLines((int)o1.X, (int)o1.Y, NodeRadius, Color.BLUE);
                Raylib.DrawLine((int)o1.X, (int)o1.Y, (int)o2.X, (int)o2.Y, Color.BLUE);
            }
            
            Raylib.DrawCircleLines((int)Bones[^1].Origin.X, (int)Bones[^1].Origin.Y, NodeRadius, Color.GOLD);
        }

        private void ForwardAndBackwardReachingInverseKinematics(Vector3 mousePosition)
        {
            var bas = Bones[^1].Origin;

            // This has the chain chase the mouse
            var target = mousePosition;
            for (var i = 0; i < Bones.Length - 1; i++)
            {
                var r = Reach(Bones[i].Origin, Bones[i + 1].Origin, target);
                Bones[i].Origin = r.Item1;
                target = r.Item2;
            }

            Bones[^1].Origin = target;

            // This anchors the chain to the last bone.
            target = bas;
            for (var i = Bones.Length - 1; i > 0; i--)
            {
                var r = Reach(Bones[i].Origin, Bones[i - 1].Origin, target);
                Bones[i].Origin = r.Item1;
                target = r.Item2;
            }
                
            Bones[0].Origin = target;
        }

        private (Vector3, Vector3) Reach(Vector3 head, Vector3 tail, Vector3 target)
        {
            var cdx = tail.X - head.X;
            var cdy = tail.Y - head.Y;
            var cdist = MathF.Sqrt(cdx * cdx + cdy * cdy);

            var sdx = tail.X - target.X;
            var sdy = tail.Y - target.Y;
            var sdist = MathF.Sqrt(sdx * sdx + sdy * sdy);

            var scale = cdist / sdist;

            return (target, new Vector3(target.X + sdx * scale, target.Y + sdy * scale, 0));
        }
    }

    private struct Bone
    {
        public float BoneLength { get; }
        public Vector3 Origin { get; set; }
        
        public Bone(float boneLength)
        {
            BoneLength = boneLength;
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
