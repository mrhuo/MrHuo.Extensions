using System;
using System.Globalization;
using System.Text;

namespace MrHuo.Extensions
{
    internal static class ObjectDisplay
    {
        internal static string NullLiteral
        {
            get
            {
                return "null";
            }
        }

        internal static string FormatLiteral(bool value)
        {
            return !value ? "false" : "true";
        }

        private static bool TryReplaceChar(char c, out string replaceWith)
        {
            replaceWith = null;
            switch (c)
            {
                case char.MinValue:
                    replaceWith = "\\0";
                    break;
                case '\a':
                    replaceWith = "\\a";
                    break;
                case '\b':
                    replaceWith = "\\b";
                    break;
                case '\t':
                    replaceWith = "\\t";
                    break;
                case '\n':
                    replaceWith = "\\n";
                    break;
                case '\v':
                    replaceWith = "\\v";
                    break;
                case '\f':
                    replaceWith = "\\f";
                    break;
                case '\r':
                    replaceWith = "\\r";
                    break;
                case '\\':
                    replaceWith = "\\\\";
                    break;
            }
            if (replaceWith != null)
                return true;
            if (!ObjectDisplay.NeedsEscaping(CharUnicodeInfo.GetUnicodeCategory(c)))
                return false;
            replaceWith = "\\u" + ((int)c).ToString("x4");
            return true;
        }

        private static bool NeedsEscaping(UnicodeCategory category)
        {
            switch (category)
            {
                case UnicodeCategory.LineSeparator:
                case UnicodeCategory.ParagraphSeparator:
                case UnicodeCategory.Control:
                case UnicodeCategory.Surrogate:
                case UnicodeCategory.OtherNotAssigned:
                    return true;
                default:
                    return false;
            }
        }

        public static string FormatLiteral(string value, ObjectDisplayOptions options)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            StringBuilder builder = new StringBuilder();
            bool flag1 = options.IncludesOption(ObjectDisplayOptions.UseQuotes);
            bool flag2 = options.IncludesOption(ObjectDisplayOptions.EscapeNonPrintableCharacters);
            bool flag3 = flag1 && !flag2 && ObjectDisplay.ContainsNewLine(value);
            if (flag1)
            {
                if (flag3)
                    builder.Append('@');
                builder.Append('"');
            }
            for (int index = 0; index < value.Length; ++index)
            {
                char ch = value[index];
                if (flag2 && CharUnicodeInfo.GetUnicodeCategory(ch) == UnicodeCategory.Surrogate)
                {
                    UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(value, index);
                    if (unicodeCategory == UnicodeCategory.Surrogate)
                        builder.Append("\\u" + ((int)ch).ToString("x4"));
                    else if (ObjectDisplay.NeedsEscaping(unicodeCategory))
                    {
                        int utf32 = char.ConvertToUtf32(value, index);
                        builder.Append("\\U" + utf32.ToString("x8"));
                        ++index;
                    }
                    else
                    {
                        builder.Append(ch);
                        builder.Append(value[++index]);
                    }
                }
                else
                {
                    string replaceWith;
                    if (flag2 && ObjectDisplay.TryReplaceChar(ch, out replaceWith))
                        builder.Append(replaceWith);
                    else if (flag1 && ch == '"')
                    {
                        if (flag3)
                        {
                            builder.Append('"');
                            builder.Append('"');
                        }
                        else
                        {
                            builder.Append('\\');
                            builder.Append('"');
                        }
                    }
                    else
                        builder.Append(ch);
                }
            }
            if (flag1)
                builder.Append('"');
            return builder.ToString();
        }

        private static bool IsNewLine(char ch)
        {
            if (ch != '\r' && ch != '\n' && (ch != '\x0085' && ch != '\x2028'))
                return ch == '\x2029';
            return true;
        }

        private static bool ContainsNewLine(string s)
        {
            foreach (char ch in s)
            {
                if (IsNewLine(ch))
                    return true;
            }
            return false;
        }

        internal static string FormatLiteral(char c, ObjectDisplayOptions options)
        {
            StringBuilder builder = new StringBuilder();
            if (options.IncludesOption(ObjectDisplayOptions.IncludeCodePoints))
            {
                builder.Append(options.IncludesOption(ObjectDisplayOptions.UseHexadecimalNumbers) ? "0x" + ((int)c).ToString("x4") : ((int)c).ToString());
                builder.Append(" ");
            }
            bool flag1 = options.IncludesOption(ObjectDisplayOptions.UseQuotes);
            bool flag2 = options.IncludesOption(ObjectDisplayOptions.EscapeNonPrintableCharacters);
            if (flag1)
                builder.Append('\'');
            string replaceWith;
            if (flag2 && ObjectDisplay.TryReplaceChar(c, out replaceWith))
                builder.Append(replaceWith);
            else if (flag1 && c == '\'')
            {
                builder.Append('\\');
                builder.Append('\'');
            }
            else
                builder.Append(c);
            if (flag1)
                builder.Append('\'');
            return builder.ToString();
        }

        internal static string FormatLiteral(
          sbyte value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            if (options.IncludesOption(ObjectDisplayOptions.UseHexadecimalNumbers))
                return "0x" + (value >= 0 ? value.ToString("x2") : ((int)value).ToString("x8"));
            return value.ToString(ObjectDisplay.GetFormatCulture(cultureInfo));
        }

        internal static string FormatLiteral(
          byte value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            if (options.IncludesOption(ObjectDisplayOptions.UseHexadecimalNumbers))
                return "0x" + value.ToString("x2");
            return value.ToString(ObjectDisplay.GetFormatCulture(cultureInfo));
        }

        internal static string FormatLiteral(
          short value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            if (options.IncludesOption(ObjectDisplayOptions.UseHexadecimalNumbers))
                return "0x" + (value >= 0 ? value.ToString("x4") : ((int)value).ToString("x8"));
            return value.ToString(ObjectDisplay.GetFormatCulture(cultureInfo));
        }

        internal static string FormatLiteral(
          ushort value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            if (options.IncludesOption(ObjectDisplayOptions.UseHexadecimalNumbers))
                return "0x" + value.ToString("x4");
            return value.ToString(ObjectDisplay.GetFormatCulture(cultureInfo));
        }

        internal static string FormatLiteral(
          int value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            if (options.IncludesOption(ObjectDisplayOptions.UseHexadecimalNumbers))
                return "0x" + value.ToString("x8");
            return value.ToString(ObjectDisplay.GetFormatCulture(cultureInfo));
        }

        internal static string FormatLiteral(
          uint value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            StringBuilder builder = new StringBuilder();
            if (options.IncludesOption(ObjectDisplayOptions.UseHexadecimalNumbers))
            {
                builder.Append("0x");
                builder.Append(value.ToString("x8"));
            }
            else
                builder.Append(value.ToString(ObjectDisplay.GetFormatCulture(cultureInfo)));
            if (options.IncludesOption(ObjectDisplayOptions.IncludeTypeSuffix))
                builder.Append('U');
            return builder.ToString();
        }

        internal static string FormatLiteral(
          long value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            StringBuilder builder = new StringBuilder();
            if (options.IncludesOption(ObjectDisplayOptions.UseHexadecimalNumbers))
            {
                builder.Append("0x");
                builder.Append(value.ToString("x16"));
            }
            else
                builder.Append(value.ToString(ObjectDisplay.GetFormatCulture(cultureInfo)));
            if (options.IncludesOption(ObjectDisplayOptions.IncludeTypeSuffix))
                builder.Append('L');
            return builder.ToString();
        }

        internal static string FormatLiteral(
          ulong value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            StringBuilder builder = new StringBuilder();
            if (options.IncludesOption(ObjectDisplayOptions.UseHexadecimalNumbers))
            {
                builder.Append("0x");
                builder.Append(value.ToString("x16"));
            }
            else
                builder.Append(value.ToString(ObjectDisplay.GetFormatCulture(cultureInfo)));
            if (options.IncludesOption(ObjectDisplayOptions.IncludeTypeSuffix))
                builder.Append("UL");
            return builder.ToString();
        }

        internal static string FormatLiteral(
          double value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            string str = value.ToString("R", ObjectDisplay.GetFormatCulture(cultureInfo));
            if (!options.IncludesOption(ObjectDisplayOptions.IncludeTypeSuffix))
                return str;
            return str + "D";
        }

        internal static string FormatLiteral(
          float value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            string str = value.ToString("R", ObjectDisplay.GetFormatCulture(cultureInfo));
            if (!options.IncludesOption(ObjectDisplayOptions.IncludeTypeSuffix))
                return str;
            return str + "F";
        }

        internal static string FormatLiteral(
          Decimal value,
          ObjectDisplayOptions options,
          CultureInfo cultureInfo = null)
        {
            string str = value.ToString(ObjectDisplay.GetFormatCulture(cultureInfo));
            if (!options.IncludesOption(ObjectDisplayOptions.IncludeTypeSuffix))
                return str;
            return str + "M";
        }

        private static CultureInfo GetFormatCulture(CultureInfo cultureInfo)
        {
            return cultureInfo ?? CultureInfo.InvariantCulture;
        }
    }
}
