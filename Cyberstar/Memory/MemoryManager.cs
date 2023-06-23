using System.Runtime.CompilerServices;

namespace Cyberstar.Memory;

public class MemoryManager
{
    public int Pages { get; private set; }
    public int PageSize { get; }
    
    private Memory<byte> Memory;
    private readonly List<AllocatorData> Allocators;
    private readonly Dictionary<Type, int> AllocatorIndexMapping;
    private int ReservedPages;

    public MemoryManager(int pageSize = 1024, int pages = 8)
    {
        Pages = pages;
        PageSize = pageSize;
        Memory = new Memory<byte>(new byte[pages * pageSize]);
        Allocators = new List<AllocatorData>();
        AllocatorIndexMapping = new Dictionary<Type, int>();
        ReservedPages = 0;
    }

    /// <summary>
    /// Requests a new allocator that will use the given number of pages. If the requested allocator
    /// will require more memory than is present and allowResize is true, the allocator will expand,
    /// otherwise we will return false.
    ///
    /// If an allocator already exists for the given type, we will return true instead of creating a
    /// new allocator. The existing allocator, however, is not guaranteed to be the requested size. 
    /// </summary>
    /// <param name="requestedPages">The number of pages the allocator will be created with</param>
    /// <param name="allowExpand">Whether or not to allow a resize in the event of not-enough-memory</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool CreateAllocator<T>(int requestedPages, bool allowExpand = true) where T : struct
    {
        var type = typeof(T);

        if (AllocatorIndexMapping.ContainsKey(type))
            return true;
        
        if (ReservedPages + requestedPages >= Pages)
            if (allowExpand)
                Expand();
            else
                return false;

        var allocator = new AllocatorData(ReservedPages * PageSize, Unsafe.SizeOf<T>(), requestedPages * PageSize, type);
        var index = Allocators.Count;
        Allocators.Add(allocator);
        AllocatorIndexMapping[type] = index;
        ReservedPages += requestedPages;
        return true;
    }

    /// <summary>
    /// Attempts to get the allocator for the given type. If it does not exist, we will return false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool TryGetAllocator<T>(out Allocator<T> allocator) where T : struct
    {
        var type = typeof(T);
        if (!AllocatorIndexMapping.TryGetValue(type, out var index))
        {
            allocator = default;
            return false;
        }

        var data = Allocators[index];
        var slice = Memory.Slice(data.Offset, data.TypeSize * data.Capacity);
        var mem = Unsafe.As<Memory<byte>, Memory<T>>(ref slice);
        allocator = new Allocator<T>(data, mem.Span);
        return true;
    }
    
    /// <summary>
    /// Resizes the backing memory such that it is twice the previous size.
    /// </summary>
    public void Expand()
    {
        Pages *= 2;
        var newMemory = new Memory<byte>(new byte[Pages * PageSize]);
        Memory.CopyTo(newMemory);
        Memory = newMemory;
    }

    public struct AllocatorData
    {
        /// <summary>
        /// The offset in the memory where the allocator should star.
        /// </summary>
        public readonly int Offset;
        /// <summary>
        /// The size of the type in the allocator.
        /// </summary>
        public readonly int TypeSize;
        /// <summary>
        /// The maximum number of type elements in the allocator
        /// </summary>
        public readonly int Capacity;
        /// <summary>
        /// The type data held in the allocator.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// The number
        /// </summary>
        public int Count;

        public AllocatorData(int offset, int typeSize, int capacity, Type type)
        {
            Offset = offset;
            TypeSize = typeSize;
            Capacity = capacity;
            Type = type;
            Count = 0;
        }
    }

    public ref struct Allocator<T> where T : struct
    {
        private ref AllocatorData SourceData;
        public Span<T> Data;

        public Allocator(AllocatorData sourceData, Span<T> data)
        {
            SourceData = sourceData;
            Data = data;
        }
    }
}