/// <summary>
/// Char 扩展方法
/// </summary>
public static class CharExtensions
{
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
}