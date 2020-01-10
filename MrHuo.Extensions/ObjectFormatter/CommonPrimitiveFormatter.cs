using System;
using System.Globalization;
using System.Reflection;

namespace MrHuo.Extensions
{
    internal abstract class CommonPrimitiveFormatter
    {
        protected abstract string NullLiteral { get; }

        protected abstract string FormatLiteral(bool value);

        protected abstract string FormatLiteral(
          string value,
          bool quote,
          bool escapeNonPrintable);

        protected abstract string FormatLiteral(
          char value,
          bool quote,
          bool escapeNonPrintable,
          bool includeCodePoints = false);

        protected abstract string FormatLiteral(sbyte value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(byte value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(short value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(ushort value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(int value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(uint value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(long value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(ulong value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(double value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(float value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(Decimal value, CultureInfo cultureInfo = null);

        protected abstract string FormatLiteral(DateTime value, CultureInfo cultureInfo = null);

        public string FormatPrimitive(object obj, CommonPrimitiveFormatterOptions options)
        {
            if (obj == ObjectFormatterHelpers.VoidValue)
                return string.Empty;
            if (obj == null)
                return this.NullLiteral;
            Type type = obj.GetType();
            if (IntrospectionExtensions.GetTypeInfo(type).IsEnum)
                return obj.ToString();
            switch (ObjectFormatterHelpers.GetPrimitiveSpecialType(type))
            {
                case SpecialType.None:
                case SpecialType.System_Object:
                case SpecialType.System_Void:
                    return null;
                case SpecialType.System_Boolean:
                    return this.FormatLiteral((bool)obj);
                case SpecialType.System_Char:
                    return this.FormatLiteral((char)obj, options.QuoteStringsAndCharacters, options.EscapeNonPrintableCharacters, options.IncludeCharacterCodePoints);
                case SpecialType.System_SByte:
                    return this.FormatLiteral((sbyte)obj, options.CultureInfo);
                case SpecialType.System_Byte:
                    return this.FormatLiteral((byte)obj, options.CultureInfo);
                case SpecialType.System_Int16:
                    return this.FormatLiteral((short)obj, options.CultureInfo);
                case SpecialType.System_UInt16:
                    return this.FormatLiteral((ushort)obj, options.CultureInfo);
                case SpecialType.System_Int32:
                    return this.FormatLiteral((int)obj, options.CultureInfo);
                case SpecialType.System_UInt32:
                    return this.FormatLiteral((uint)obj, options.CultureInfo);
                case SpecialType.System_Int64:
                    return this.FormatLiteral((long)obj, options.CultureInfo);
                case SpecialType.System_UInt64:
                    return this.FormatLiteral((ulong)obj, options.CultureInfo);
                case SpecialType.System_Decimal:
                    return this.FormatLiteral((Decimal)obj, options.CultureInfo);
                case SpecialType.System_Single:
                    return this.FormatLiteral((float)obj, options.CultureInfo);
                case SpecialType.System_Double:
                    return this.FormatLiteral((double)obj, options.CultureInfo);
                case SpecialType.System_String:
                    return this.FormatLiteral((string)obj, options.QuoteStringsAndCharacters, options.EscapeNonPrintableCharacters);
                case SpecialType.System_DateTime:
                    return this.FormatLiteral((DateTime)obj, options.CultureInfo);
                default:
                    throw new NotSupportedException(ObjectFormatterHelpers.GetPrimitiveSpecialType(type).ToString());
            }
        }
    }
}
