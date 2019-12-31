using System;
using System.Linq;
using System.Reflection;

/// <summary>
/// 反射扩展方法
/// </summary>
public static class ReflectExtensions
{
    /// <summary>
    /// 判断某个 ICustomAttributeProvider 对象实例是否具有 TAttribute 类型的特性
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="provider"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static bool HasAttribute<TAttribute>(this ICustomAttributeProvider provider, bool inherit = false)
        where TAttribute : Attribute
    {
        var attrs = provider.GetAttributes<TAttribute>(inherit);
        return attrs != null && attrs.Length > 0;
    }

    /// <summary>
    /// 获取某个 ICustomAttributeProvider 对象实例的所有 TAttribute 类型的特性
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="provider"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static TAttribute[] GetAttributes<TAttribute>(this ICustomAttributeProvider provider, bool inherit = false)
        where TAttribute : Attribute
    {
        return provider.GetCustomAttributes(typeof(TAttribute), inherit) as TAttribute[];
    }

    /// <summary>
    /// 获取元素具有的指定特性
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="provider"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider provider, bool inherit = false)
        where TAttribute : Attribute
    {
        return provider.GetAttributes<TAttribute>(inherit)?.FirstOrDefault();
    }

    /// <summary>
    /// 将一个对象的每一个公开可读属性调用委托处理
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyAction"></param>
    public static void EveryPropertyInvoke<T>(this T obj, Action<PropertyInfo, Object> propertyAction)
        where T : class
    {
        if (propertyAction == null)
        {
            return;
        }
        var allProperties = obj.GetType().GetProperties();
        foreach (var item in allProperties)
        {
            if (item.CanRead)
            {
                var value = item.GetValue(obj);
                propertyAction(item, value);
            }
        }
    }
}