using MrHuo.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

/// <summary>
/// Object 扩展方法
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// 将任意类型对象序列化成 JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson(this object obj, Formatting formatting = Formatting.None, JsonSerializerSettings settings = null)
    {
        return JsonConvert.SerializeObject(obj, formatting, settings ?? new JsonSerializerSettings());
    }

    /// <summary>
    /// 将一个 JSON 字符串反序列化成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static T FromJson<T>(this string json, JsonSerializerSettings settings = null)
    {
        return JsonConvert.DeserializeObject<T>(json, settings ?? new JsonSerializerSettings());
    }

    #region [Mapper]
    /// <summary>
    /// 将一个对象映射成为另外一个对象，相同名称的属性自动赋值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T MapTo<T>(this object obj) where T : class, new()
    {
        var allProperties = obj.GetType().GetProperties();
        var destProperties = typeof(T).GetProperties();
        var ret = new T();
        foreach (var item in allProperties)
        {
            var dest = destProperties.SingleOrDefault(p => p.Name == item.Name);
            if (dest != null && item.CanRead && dest.CanWrite && item.PropertyType == dest.PropertyType)
            {
                var value = item.GetValue(obj, null);
                dest.SetValue(ret, value);
            }
        }
        return ret;
    }

    /// <summary>
    /// 使用原对象中的某些属性，生成新对象
    /// </summary>
    /// <typeparam name="TSource">任意对象类型</typeparam>
    /// <typeparam name="TDest">新对象类型</typeparam>
    /// <param name="obj"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static TDest MapTo<TSource, TDest>(this TSource obj, Func<TSource, TDest> func)
        where TDest: class
    {
        return func(obj);
    }

    /// <summary>
    /// 对一个列表的多个元素执行 MapTo 操作，返回新的对象 List
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="sources"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static List<TDest> MapTo<TSource, TDest>(this IEnumerable<TSource> sources, Func<TSource, TDest> func)
        where TDest: class
    {
        var list = new List<TDest>();
        foreach (var item in sources)
        {
            list.Add(func(item));
        }
        return list;
    }
    #endregion

    #region [Dump]
    /// <summary>
    /// 将对象转化为 String 表示，并写入 Console Window
    /// </summary>
    /// <param name="obj"></param>
    public static string DumpToConsole(this object obj)
    {
        var str = DumpToString(obj);
        Console.WriteLine(str);
        return str;
    }
    /// <summary>
    /// 将对象转化为 String 表示，并写入 Debug Window
    /// </summary>
    /// <param name="obj"></param>
    public static string DumpToDebug(this object obj)
    {
        var str = DumpToString(obj);
        Debug.WriteLine(str);
        return str;
    }

    private static readonly ObjectFormatter objectFormatter = ObjectFormatter.Instance;
    /// <summary>
    /// 将任意对象转化为 String 表示
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string DumpToString(this object obj)
    {
        return objectFormatter.FormatObject(obj);
    }
    #endregion
}