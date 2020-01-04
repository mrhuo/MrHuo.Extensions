using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
}