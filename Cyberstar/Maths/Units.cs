using System.Runtime.CompilerServices;

namespace Cyberstar.Maths;

public class Units
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float DegreesToRadians(float degrees) => MathF.PI / 180f * degrees;
}