namespace Cyberstar.Collections;

public class HashTable<TKey, TValue>
{
    private TValue[] _data;
    private float _loadFactor;

    /// <summary>
    /// Creates a new HashTable with a capacity as close to the given value as possible, but
    /// is a prime number.
    /// </summary>
    /// <param name="capacity"></param>
    /// <param name="loadFactor">The minimum percent of full that the hash table must be before resizing.</param>
    public HashTable(uint capacity, float loadFactor = 0.65f)
    {
        var size = GetClosestPrime(capacity);
        _data = new TValue[size];
        _loadFactor = loadFactor;
    }

    private static uint GetClosestPrime(uint number)
    {
        if (number % 2 == 0) number -= 1;
        if (number < 2) return 2;

        for (var i = number; i > 2; i -= 2)
        {
            if (IsPrime(i))
                return i;
        }

        return 2;
    }

    private static bool IsPrime(uint number)
    {
        if (number == 2) return true;
        if (number % 2 == 0) return false;
        
        for (var i = 3; i * i <= number; i+= 2)
            if (number % i == 0)
                return false;

        return true;
    }
}