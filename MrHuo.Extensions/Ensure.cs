using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
/// 检查输入参数
/// </summary>
internal static class Ensure
{
    /// <summary>
    /// 检查输入参数是否为 null，如为 null，则抛出 ArgumentNullException 异常
    /// </summary>
    /// <param name="obj">任意对象</param>
    /// <param name="paramName">对象参数名，默认为 nameof(obj)</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void NotNull(object obj, string paramName = null)
    {
        paramName = paramName ?? nameof(obj);
        if (obj ==null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    /// <summary>
    /// 检查输入字符串是否为 null 或为空字符串，如为空，则抛出 ArgumentNullException 异常
    /// </summary>
    /// <param name="str">任意字符串</param>
    /// <param name="paramName">对象参数名，默认为 nameof(str)</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void NotNullOrEmpty(string str, string paramName = null)
    {
        paramName = paramName ?? nameof(str);
        if (string.IsNullOrEmpty(str))
        {
            throw new ArgumentNullException(paramName);
        }
    }

    /// <summary>
    /// 检查输入的文件路径是否存在，如不存在，则抛出 FileNotFoundException 异常
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <exception cref="FileNotFoundException"></exception>
    public static void FileExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
           throw new FileNotFoundException($"文件[{filePath}]不存在", filePath);            
        }
    }

    /// <summary>
    /// 检查输入的文件的扩展名
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="extensions"></param>
    public static void FileExtensions(string filePath, params string[] extensions)
    {
        if (!string.IsNullOrEmpty(filePath) && extensions.Length > 0)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            if (!extensions.Any(p=>ext == p))
            {
                throw new NotSupportedException($"不支持的扩展名为[{ext}]的文件，允许的扩展名为[{extensions.Join(",")}]");
            }
        }
    }

    /// <summary>
    /// 检查输入的文件夹路径是否存在，如不存在，则抛出 DirectoryNotFoundException 异常
    /// </summary>
    /// <param name="dirPath">文件夹路径</param>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public static void DirectoryExist(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            throw new DirectoryNotFoundException($"文件夹[{dirPath}]不存在");
        }
    }

    /// <summary>
    /// 检查输入的集合 Count 是否等于0，如等于0，则抛出 Exception 异常
    /// </summary>
    /// <param name="collection"></param>
    /// <exception cref="Exception"></exception>
    public static void HasValue(ICollection collection)
    {
        if (collection.Count == 0)
        {
            throw new Exception($"集合内没有数据");
        }
    }
}