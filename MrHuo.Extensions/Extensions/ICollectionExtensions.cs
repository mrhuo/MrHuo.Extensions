using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ICollection 扩展方法
/// </summary>
public static class ICollectionExtensions
{
    /// <summary>
    /// 判断一个集合是否有元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool HasValue<T>(this ICollection<T> value)
    {
        return value.Any();
    }
}