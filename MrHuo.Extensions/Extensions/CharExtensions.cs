using System.Collections.Generic;
using System.Linq;
/// <summary>
/// Char 扩展方法
/// </summary>
public static class CharExtensions
{
    /// <summary>
    /// 小写转大写
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static char ToUpper(this char ch)
    {
        if (ch.IsLower())
        {
            ch = (char)(ch - 32);
        }
        return ch;
    }

    /// <summary>
    /// 大写转小写
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static char ToLower(this char ch)
    {
        if (ch.IsUpper())
        {
            ch = (char)(ch + 32);
        }
        return ch;
    }

    #region [IsUpper]
    /// <summary>
    /// 某个字符是否大写
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static bool IsUpper(this char ch)
    {
        return ch >= 65 && ch <= 65 + 26;
    }
    #endregion

    #region [IsLower]
    /// <summary>
    /// 某个字符是否小写
    /// </summary>
    /// <param name="ch">字符</param>
    /// <returns></returns>
    public static bool IsLower(this char ch)
    {
        return ch >= 97 && ch <= 97 + 26;
    }
    #endregion

    #region [ToString]
    /// <summary>
    /// 转化成拼接后的字符串
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string ToString(this IEnumerable<char> enumerable, string separator)
    {
        return string.Join(separator, enumerable.ToArray());
    }
    #endregion
}