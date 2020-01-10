using System.Globalization;

namespace MrHuo.Extensions
{
    internal class CommonPrimitiveFormatterOptions
    {
        public bool IncludeCharacterCodePoints { get; }

        public bool QuoteStringsAndCharacters { get; } = true;

        public bool EscapeNonPrintableCharacters { get; } = false;

        public CultureInfo CultureInfo { get; } = CultureInfo.CurrentUICulture;
    }
}
