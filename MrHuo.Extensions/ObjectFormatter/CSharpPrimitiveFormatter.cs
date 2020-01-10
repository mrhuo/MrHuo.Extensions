using System;
using System.Globalization;

namespace MrHuo.Extensions
{
    internal class CSharpPrimitiveFormatter : CommonPrimitiveFormatter
    {
        protected override string NullLiteral
        {
            get
            {
                return ObjectDisplay.NullLiteral;
            }
        }

        protected override string FormatLiteral(bool value)
        {
            return ObjectDisplay.FormatLiteral(value);
        }

        protected override string FormatLiteral(
          string value,
          bool useQuotes,
          bool escapeNonPrintable)
        {
            ObjectDisplayOptions objectDisplayOptions = ObjectFormatterHelpers.GetObjectDisplayOptions(useQuotes, escapeNonPrintable, false);
            return ObjectDisplay.FormatLiteral(value, objectDisplayOptions);
        }

        protected override string FormatLiteral(
          char c,
          bool useQuotes,
          bool escapeNonPrintable,
          bool includeCodePoints = false)
        {
            ObjectDisplayOptions objectDisplayOptions = ObjectFormatterHelpers.GetObjectDisplayOptions(useQuotes, escapeNonPrintable, includeCodePoints);
            return ObjectDisplay.FormatLiteral(c, objectDisplayOptions);
        }

        protected override string FormatLiteral(sbyte value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectFormatterHelpers.GetObjectDisplayOptions(false, false, false), cultureInfo);
        }

        protected override string FormatLiteral(byte value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectFormatterHelpers.GetObjectDisplayOptions(false, false, false), cultureInfo);
        }

        protected override string FormatLiteral(short value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectFormatterHelpers.GetObjectDisplayOptions(false, false, false), cultureInfo);
        }

        protected override string FormatLiteral(ushort value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectFormatterHelpers.GetObjectDisplayOptions(false, false, false), cultureInfo);
        }

        protected override string FormatLiteral(int value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectFormatterHelpers.GetObjectDisplayOptions(false, false, false), cultureInfo);
        }

        protected override string FormatLiteral(uint value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectFormatterHelpers.GetObjectDisplayOptions(false, false, false), cultureInfo);
        }

        protected override string FormatLiteral(long value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectFormatterHelpers.GetObjectDisplayOptions(false, false, false), cultureInfo);
        }

        protected override string FormatLiteral(ulong value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectFormatterHelpers.GetObjectDisplayOptions(false, false, false), cultureInfo);
        }

        protected override string FormatLiteral(double value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectDisplayOptions.None, cultureInfo);
        }

        protected override string FormatLiteral(float value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectDisplayOptions.None, cultureInfo);
        }

        protected override string FormatLiteral(Decimal value, CultureInfo cultureInfo = null)
        {
            return ObjectDisplay.FormatLiteral(value, ObjectDisplayOptions.None, cultureInfo);
        }

        protected override string FormatLiteral(DateTime value, CultureInfo cultureInfo = null)
        {
            return null;
        }
    }
}
