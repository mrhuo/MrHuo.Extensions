using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

/// <summary>
/// NameValueCollection 扩展方法
/// </summary>
public static class NameValueCollectionExtensions
{
    /// <summary>
    /// 将一个 NameValueCollection 转化成字典
    /// </summary>
    /// <param name="nameValueCollection"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ToDictionary(this NameValueCollection nameValueCollection)
    {
        var dict = new Dictionary<string, string>();
        foreach (var key in nameValueCollection.AllKeys)
        {
            dict[key] = nameValueCollection[key];
        }
        return dict;
    }
}