using System;

namespace MrHuo.Extensions
{
    [Flags]
    internal enum ObjectDisplayOptions
    {
        None = 0,
        IncludeCodePoints = 1,
        IncludeTypeSuffix = 2,
        UseHexadecimalNumbers = 4,
        UseQuotes = 8,
        EscapeNonPrintableCharacters = 16, // 0x00000010
    }
}
