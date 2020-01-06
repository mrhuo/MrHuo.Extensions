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

    /// <summary>
    /// 直接获取对象的属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static PropertyInfo GetProperty<T>(this T obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName);
    }

    /// <summary>
    /// 直接对对象的属性赋值
    /// <para>属性不存在或不可写时，不执行任何操作</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <param name="data"></param>
    public static void SetValue<T>(this T obj, string propertyName, object data)
    {
        var property = obj.GetProperty(propertyName);
        if (property != null && property.CanWrite)
        {
            property.SetValue(obj, data);
        }
    }

    /// <summary>
    /// 直接获取对象属性的值
    /// <para>属性不存在或不可读，返回 null</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static object GetValue<T>(this T obj, string propertyName)
    {
        var property = obj.GetProperty(propertyName);
        if (property == null || !property.CanRead)
        {
            return null;
        }
        return property.GetValue(obj);
    }
}