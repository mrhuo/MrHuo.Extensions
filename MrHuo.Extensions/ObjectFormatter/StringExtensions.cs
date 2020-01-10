using System;
using System.Collections.Generic;

namespace MrHuo.Extensions
{
    internal static class StringExtensions
    {
        private static readonly Func<char, char> s_toLower = new Func<char, char>(char.ToLower);
        private static readonly Func<char, char> s_toUpper = new Func<char, char>(char.ToUpper);

        internal static string GetNumeral(int number)
        {
            return $"{number}";
        }

        internal static int IndexOfBalancedParenthesis(
          this string str,
          int openingOffset,
          char closing)
        {
            char ch1 = str[openingOffset];
            int num = 1;
            for (int index = openingOffset + 1; index < str.Length; ++index)
            {
                char ch2 = str[index];
                if (ch2 == ch1)
                    ++num;
                else if (ch2 == closing)
                {
                    --num;
                    if (num == 0)
                        return index;
                }
            }
            return -1;
        }
    }
}
