using System;
using System.Collections.Generic;
using System.Text;

public static class NumberExtensions
{
    #region [FormatMemorySize]
    /// <summary>
    /// 获取友好的格式化后的内存大小表示字符串
    /// </summary>
    /// <param name="sizeInBytes"></param>
    /// <returns></returns>
    public static string GetFriendlyMemorySize(double sizeInBytes)
    {
        var num = sizeInBytes;
        var strArray = new string[5] { "B", "KB", "MB", "GB", "TB" };
        int index;
        for (index = 0; num >= 1024.0 && index < strArray.Length - 1; num /= 1024.0)
            ++index;
        return string.Format(string.Format("{0:0.00} {1}", num, strArray[index]));
    }

    /// <summary>
    /// 获取友好的格式化后的内存大小表示字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormatMemorySize(this int value)
    {
        return GetFriendlyMemorySize(value);
    }

    /// <summary>
    /// 获取友好的格式化后的内存大小表示字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormatMemorySize(this long value)
    {
        return GetFriendlyMemorySize(value);
    }

    /// <summary>
    /// 获取友好的格式化后的内存大小表示字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormatMemorySize(this double value)
    {
        return GetFriendlyMemorySize(value);
    }

    /// <summary>
    /// 获取友好的格式化后的内存大小表示字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormatMemorySize(this float value)
    {
        return GetFriendlyMemorySize(value);
    }
    #endregion
}