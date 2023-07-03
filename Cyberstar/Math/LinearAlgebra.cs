using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cyberstar.Math;

/// <summary>
/// Provides most of the linear algebra functions that are needed for 2 and 3 dimensional maths.
/// </summary>
public static class LinearAlgebra
{
    /// <summary>
    /// Performs linear interpolation between the first and second points where beta is the percent distance
    /// between the first and second points. 
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="beta">A [0-1] percent of interpolation. 0 returns first, 1 returns second.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Vector2 Lerp(this Vector2 first, Vector2 second, float beta) => Vector2.Lerp(first, second, beta);
    
    /// <summary>
    /// Performs linear interpolation between the first and second points where beta is the percent distance
    /// between the first and second points. 
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="beta">A [0-1] percent of interpolation. 0 returns first, 1 returns second.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Vector3 Lerp(this Vector3 first, Vector3 second, float beta) => Vector3.Lerp(first, second, beta);

    /// <summary>
    /// Performs spherical interpolation between the first and second points where beta is the percent distance
    /// between the first and second points. 
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="beta">A [0-1] percent of interpolation. 0 returns first, 1 returns second.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Vector2 Slerp(this Vector2 first, Vector2 second, float beta)
    {
        var omega = first.AngleTo(second);
        var iNumerator = MathF.Sin((1 - beta) * omega);
        var jNumerator = MathF.Sin(beta * omega);
        var sineOmega = MathF.Sin(omega);
        return (iNumerator / sineOmega) * first + (jNumerator / sineOmega) * second;
    }
    
    /// <summary>
    /// Performs spherical interpolation between the first and second points where beta is the percent distance
    /// between the first and second points. 
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="beta">A [0-1] percent of interpolation. 0 returns first, 1 returns second.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Vector3 Slerp(this Vector3 first, Vector3 second, float beta)
    {
        var omega = first.AngleTo(second);
        var iNumerator = MathF.Sin((1 - beta) * omega);
        var jNumerator = MathF.Sin(beta * omega);
        var sineOmega = MathF.Sin(omega);
        return (iNumerator / sineOmega) * first + (jNumerator / sineOmega) * second;
    }

    /// <summary>
    /// Normalizes the vector to a unit length.
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Vector2 Normalize(this Vector2 self)
    {
        var len = self.Length();
        return new Vector2(self.X / len, self.Y / len);
    }
    
    /// <summary>
    /// Computes the angle from the first vector to the second vector.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float AngleTo(this Vector3 first, Vector3 second) => MathF.Acos(first.Dot(second) / (first.Length() * second.Length()));
    
    /// <summary>
    /// Computes the angle from the first vector to the second vector.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float AngleTo(this Vector2 first, Vector2 second) => MathF.Acos(first.Dot(second) / (first.Length() * second.Length()));

    /// <summary>
    /// Computes the dot product between two vectors.
    ///
    /// Dot products and represent a number of things based on their context.
    ///     * Collinear- (a*b) = |a||b| = ab (i.e., the angle between the vectors is exactly 0 degrees)
    ///     * Collinear [but opposite]- (a*b) = -ab (i.e., the angle between them is exactly 180 degrees)
    ///     * Perpendicular: (a*b) = 0 (i.e., The angle between the vectors is exactly 90 degrees)
    ///     * Same direction: (a*b) > 0 (i.e., The angle between them is less than 90 degrees)
    ///     * Opposite direction: (a*b) < 0 (i.e., The angle between them greater than 90 degrees)
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float Dot(this Vector2 first, Vector2 second) => first.X * second.X + first.Y * second.Y;
    
    /// <summary>
    /// Computes the dot product between two vectors.
    ///
    /// Dot products and represent a number of things based on their context.
    ///     * Collinear- (a*b) = |a||b| = ab (i.e., the angle between the vectors is exactly 0 degrees)
    ///     * Collinear [but opposite]- (a*b) = -ab (i.e., the angle between them is exactly 180 degrees)
    ///     * Perpendicular: (a*b) = 0 (i.e., The angle between the vectors is exactly 90 degrees)
    ///     * Same direction: (a*b) > 0 (i.e., The angle between them is less than 90 degrees)
    ///     * Opposite direction: (a*b) < 0 (i.e., The angle between them greater than 90 degrees)
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float Dot(this Vector3 first, Vector3 second) => first.X * second.X + first.Y * second.Y + first.Z * second.Z;

    /// <summary>
    /// Returns the cross product of the two vectors.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Vector3 Cross(this Vector3 first, Vector3 second) => Vector3.Cross(first, second);

    /// <summary>
    /// Projects the vector onto the given line returning the projected "shadow" of the vector onto the line.
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="projectionLine"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Vector2 ProjectOnTo(this Vector2 vector, Vector2 projectionLine) => vector.Dot(projectionLine) * Vector2.Normalize(projectionLine);
    
    /// <summary>
    /// Projects the vector onto the given line returning the projected "shadow" of the vector onto the line.
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="projectionLine"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Vector3 ProjectOnTo(this Vector3 vector, Vector3 projectionLine) => vector.Dot(projectionLine) * Vector3.Normalize(projectionLine);
}