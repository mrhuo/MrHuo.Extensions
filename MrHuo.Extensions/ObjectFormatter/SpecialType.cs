namespace MrHuo.Extensions
{
    internal enum SpecialType : sbyte
    {
        None = 0,
        System_Object = 1,
        System_Enum = 2,
        System_MulticastDelegate = 3,
        System_Delegate = 4,
        System_ValueType = 5,
        System_Void = 6,
        System_Boolean = 7,
        System_Char = 8,
        System_SByte = 9,
        System_Byte = 10, // 0x0A
        System_Int16 = 11, // 0x0B
        System_UInt16 = 12, // 0x0C
        System_Int32 = 13, // 0x0D
        System_UInt32 = 14, // 0x0E
        System_Int64 = 15, // 0x0F
        System_UInt64 = 16, // 0x10
        System_Decimal = 17, // 0x11
        System_Single = 18, // 0x12
        System_Double = 19, // 0x13
        System_String = 20, // 0x14
        System_IntPtr = 21, // 0x15
        System_UIntPtr = 22, // 0x16
        System_Array = 23, // 0x17
        System_Collections_IEnumerable = 24, // 0x18
        System_Collections_Generic_IEnumerable_T = 25, // 0x19
        System_Collections_Generic_IList_T = 26, // 0x1A
        System_Collections_Generic_ICollection_T = 27, // 0x1B
        System_Collections_IEnumerator = 28, // 0x1C
        System_Collections_Generic_IEnumerator_T = 29, // 0x1D
        System_Collections_Generic_IReadOnlyList_T = 30, // 0x1E
        System_Collections_Generic_IReadOnlyCollection_T = 31, // 0x1F
        System_Nullable_T = 32, // 0x20
        System_DateTime = 33, // 0x21
        System_Runtime_CompilerServices_IsVolatile = 34, // 0x22
        System_IDisposable = 35, // 0x23
        System_TypedReference = 36, // 0x24
        System_ArgIterator = 37, // 0x25
        System_RuntimeArgumentHandle = 38, // 0x26
        System_RuntimeFieldHandle = 39, // 0x27
        System_RuntimeMethodHandle = 40, // 0x28
        System_RuntimeTypeHandle = 41, // 0x29
        System_IAsyncResult = 42, // 0x2A
        Count = 43, // 0x2B
        System_AsyncCallback = 43, // 0x2B
    }
}
