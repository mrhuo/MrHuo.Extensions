using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// Byte 扩展方法
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// 转化成字符串，默认 UTF8 编码
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string ToStringEx(this byte[] bytes, Encoding encoding = null)
    {
        encoding = encoding ?? Encoding.UTF8;
        return encoding.GetString(bytes);
    }

    /// <summary>
    /// 转化成16进制的字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToHex(this byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (var item in bytes)
        {
            sb.Append(item.ToString("x2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// 将字节数组写入到文件
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="fileName"></param>
    public static void ToFile(this byte[] bytes, string fileName)
    {
        using (var ms = new MemoryStream(bytes))
        {
            ms.ToFile(fileName);
        }
    }

    /// <summary>
    /// 将字节数组追加到文件，如果不存在，文件自动创建
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="fileName"></param>
    public static void AppendToFile(this byte[] bytes, string fileName)
    {
        using (var ms = new MemoryStream(bytes))
        {
            ms.AppendToFile(fileName);
        }
    }
}