using System.Numerics;
using Cyberstar.Core;
using Cyberstar.Engine;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.Engine.Model3d;
using Cyberstar.Engine.Scenes;
using Raylib_cs;

namespace Cyberstar.Game.Scenes;

public class ModelScene : Scene
{
    private const float Speed = 5;
    private const float Fps = 1f / 60;
    
    private Camera3D _camera;
    private Model _model;
    private int _boneIndex;
    private Transform _baseTransform;
    public ModelScene(IEngine engine) : base(engine)
    {
        _camera = new Camera3D(Vector3.Zero - Vector3.UnitZ * 15, Vector3.Zero, Vector3.UnitY, 90, CameraProjection.CAMERA_PERSPECTIVE);
        
        // Convert FBX with anims to glb
        // https://github.com/facebookincubator/FBX2glTF
        // Animated characters
        // https://www.mixamo.com/#/?type=Motion%2CMotionPack
        
        if (engine.AssetManager.TryLoadModel("testing/robot.glb", out var model))
        {
            _model = model;
            var bb = Raylib.GetModelBoundingBox(_model);
            var normalFactor = bb.max - bb.min;
            var scale = Matrix4x4.CreateScale(Vector3.One / normalFactor);
            // _model.transform = Matrix4x4.Multiply(_model.transform, scale);
            _model.ListBones();

            unsafe
            {
                if (_model.TryFindBone("Head".AsSpan(), out _boneIndex))
                {
                    _baseTransform = _model.bindPose[_boneIndex];
                }
            }

            // if (engine.AssetManager.TryLoadAnimations("testing/robot.glb", out var modelAnimations))
            // {
            //     _animationCollection = modelAnimations;
            //     // Raylib.UpdateModelAnimation(_model, _animationCollection.Animations[_animation], 1);
            // }
        }
        else
        {
            ;
        }
    }

    private float _deltaTime;
    private int _frame;
    private float _rotation = Fps;
    private int _animation = 0;

    public override void PerformTick(FrameTiming frameTiming)
    {
        _deltaTime += frameTiming.DeltaTime;
        // if (_deltaTime > Fps)
        // {
        //     _frame++;
        //     if (_frame >= _animationCollection.Animations[_animation].frameCount)
        //     {
        //         _frame = 0;
        //         // _animation = (_animation + 1) % _animationCollection.Animations.Count;
        //     }
        //     _deltaTime -= Fps;
        //     Raylib.UpdateModelAnimation(_model, _animationCollection.Animations[_animation], _frame);
        // }

        unsafe
        {
            if (_boneIndex > 0)
            {
                _baseTransform.scale = _baseTransform.scale * 2 * frameTiming.DeltaTime;
                _model.bindPose[_boneIndex] = _baseTransform;
            }
        }


        var rotation = Matrix4x4.CreateRotationY(_rotation * (MathF.PI / 180.0f));
        _model.transform = Matrix4x4.Multiply(rotation, _model.transform);
        
        Raylib.BeginMode3D(_camera);
        Raylib.DrawModel(_model, Vector3.Zero, 1, Color.RAYWHITE);
        Raylib.EndMode3D();
    }
}