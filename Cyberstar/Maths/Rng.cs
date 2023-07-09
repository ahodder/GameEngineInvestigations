namespace Cyberstar.Maths;

/// <summary>
/// A simple implementation of a random number generator.
/// </summary>
public struct Rng
{
    private uint _a;
    private uint _b;

    public Rng(uint seed = 0)
    {
        _a = 0;
        _b = seed;
    }

    public void SetSeed(uint seed)
    {
        _a = _b;
        _b = seed;
    }

    /// <summary>
    /// Gets a new float whose range is [0, 1]
    /// </summary>
    /// <returns></returns>
    public float NextFloat()
    {
        return 1f / Xoroshiro();
    }

    public int NextInt(int max)
    {
        return (int)(NextFloat() * max);
    }

    private uint Xoroshiro()
    {
        var r = _a * 0x9e3779bb;
        r = (r << 5 | r >>> 27) * 5;
        _b = _b ^ _a;
        _a = _b ^ (_a << 26 | _a >>> 6) ^ _b << 9;
        _b = _b << 13 | _b >>> 19;
        return r / uint.MaxValue;
    }
}