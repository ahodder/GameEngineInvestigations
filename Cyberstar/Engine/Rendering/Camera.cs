using System.Numerics;
using Cyberstar.Maths;

namespace Cyberstar.Engine.Rendering;

/// <summary>
/// A Camera that is used scale rendering and provide a field of vision.
/// </summary>
public class Camera
{
    /// <summary>
    /// The position of the camera.
    /// </summary>
    public Vector3 Position { get; private set; }
    /// <summary>
    /// The direction the camera is facing.
    /// </summary>
    public Vector3 FacingVector { get; private set; }

    /// <summary>
    /// The Vector that indicates the Up orientation for the camera.
    /// </summary>
    public Vector3 CameraUp { get; private set; }

    public float ZoomFactor { get; private set; }

    public float AspectRatio { get; private set; }

    public float NearPlaneDistance { get; private set; }
    
    public float FarPlaneDistance { get; private set; }

    public Matrix4x4 View => Matrix4x4.CreateLookAt(Position, Position + FacingVector, CameraUp);
    public Matrix4x4 Projection => Matrix4x4.CreatePerspectiveFieldOfView(Units.DegreesToRadians(ZoomFactor), AspectRatio, NearPlaneDistance, FarPlaneDistance);

    public Camera(Vector3 position,
        Vector3 facing,
        Vector3 cameraUp,
        float zoom = 1f,
        float aspectRatio = 1.5f,
        float nearPlaneDistance = 0.1f,
        float farPlaneDistance = 100f)
    {
        Position = position;
        FacingVector = facing;
        CameraUp = cameraUp;
        ZoomFactor = zoom;
        AspectRatio = aspectRatio;
        NearPlaneDistance = nearPlaneDistance;
        FarPlaneDistance = farPlaneDistance;
    }

    public Matrix4x4 CalculateTransform(Matrix4x4 model)
    {
        return (Projection * View) * model;
    }

    public Camera Zoom(float zoom)
    {
        ZoomFactor = zoom;
        return this;
    }
}