using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 字典扩展方法
/// </summary>
public static class DictionaryExtensions
{
    #region [Get]
    /// <summary>
    /// 从字典中获取指定 Key 的值，如果 Key 不存在，返回 default(TValue)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TValue Get<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue defaultValue = default(TValue))
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }
        return defaultValue;
    }

    /// <summary>
    /// 从字典中获取指定索引的值，如果 Key 不存在，返回 default(TValue)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="index"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TValue Get<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        int index,
        TValue defaultValue = default(TValue))
    {
        if (index < 0 || index > dictionary.Count - 1)
        {
            return defaultValue;
        }
        var i = 0;
        var enumerator = dictionary.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (i++ == index)
            {
                return enumerator.Current.Value;
            }
        }
        return defaultValue;
    }
    #endregion

    /// <summary>
    /// 从字典中移除多个 key，返回移除个数
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static int Remove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, params TKey[] keys)
    {
        var removed = 0;
        foreach (var key in keys)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                removed++;
            }
        }
        return removed;
    }

    /// <summary>
    /// 获取字典的所有 Key，返回 List<TKey>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static List<TKey> Keys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        return dictionary.Keys.ToList();
    }

    /// <summary>
    /// 获取字典的所有 Value，返回 List<TValue>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static List<TValue> Values<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        return dictionary.Values.ToList();
    }

    /// <summary>
    /// 将一个字典拼接成一个字符串
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="joinSeparator">是每个元素之间的分隔符</param>
    /// <param name="keyValueSeparator">是元素键值对之间的分隔符</param>
    /// <returns></returns>
    public static string Join<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, string joinSeparator, string keyValueSeparator)
    {
        var arr = (from kv in dictionary
                   select $"{kv.Key}{keyValueSeparator}{kv.Value}").ToArray();
        return string.Join(joinSeparator, arr);
    }

    /// <summary>
    /// 将一个字典转化成 QueryString 字符串
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static string ToQueryString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, bool urlEncodeValue = true)
    {
        var arr = (from kv in dictionary
                   let v = $"{kv.Value}".UrlEncode()
                   select $"{kv.Key}={kv.Value}").ToArray();
        return string.Join("&", arr);
    }
}