using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 类型转换相关的扩展方法
/// </summary>
public static class ConvertExtensions
{
    #region [GetDefaultValueOfType]
    /// <summary>
    /// 获取指定类型的默认值
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetDefaultValueOfType(this Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
    #endregion

    #region [To]
    /// <summary>
    /// 将一个值转换为任意类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="throwException"></param>
    /// <returns></returns>
    public static object To(this object value, Type targetType, bool throwException = false)
    {
        try
        {
            var valueString = value.ToString();
            if (targetType.IsEnum == true)
            {
                var result = Enum.Parse(targetType, valueString, true);
                return result;
            }
            var convertible = value as IConvertible;
            if (convertible != null)
            {
                if (typeof(IConvertible).IsAssignableFrom(targetType) == true)
                {
                    var result = convertible.ToType(targetType, null);
                    return result;
                }
                //判断convertsionType是否为nullable泛型类
                else if (targetType.IsGenericType &&
                    targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return Convert.ChangeType(convertible, Nullable.GetUnderlyingType(targetType));
                }
            }
            if (typeof(Guid) == targetType)
            {
                var result = Guid.Parse(valueString);
                return result;
            }
            else if (typeof(string) == targetType)
            {
                var result = valueString;
                return true;
            }
            return null;
        }
        catch (Exception ex)
        {
            if (throwException)
            {
                throw ex;
            }
        }
        return targetType.GetDefaultValueOfType();
    }

    /// <summary>
    /// 任意类型的转化，如果 throwException=true 此方法会抛出异常，默认失败后不会抛出异常
    /// </summary>
    /// <typeparam name="TReturnType"></typeparam>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <param name="throwException">是否抛出异常，默认否</param>
    /// <returns></returns>
    public static TReturnType To<TReturnType>(this object value, TReturnType defaultValue = default(TReturnType), bool throwException = false)
    {
        if (value == null)
        {
            return defaultValue;
        }
        try
        {
            return (TReturnType)To(value, typeof(TReturnType), throwException);
        }
        catch (Exception ex)
        {
            if (throwException)
            {
                throw ex;
            }
        }
        return defaultValue;
    }
    /// <summary>
    /// 转化一个对象到 int 类型，此方法转化失败后不会抛异常
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int ToInt32(this object value)
    {
        return value.To<int>(default(int), false);
    }

    /// <summary>
    /// 转化一个对象到 long 类型，此方法转化失败后不会抛异常
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long ToInt64(this object value)
    {
        return value.To<long>(default(long), false);
    }

    /// <summary>
    /// 转化一个对象到 short 类型，此方法转化失败后不会抛异常
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static short ToShort(this object value)
    {
        return value.To<short>(default(short), false);
    }

    /// <summary>
    /// 转化一个对象到 DateTime 类型，此方法转化失败后不会抛异常
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this object value)
    {
        return value.To<DateTime>(default(DateTime), false);
    }

    /// <summary>
    /// 转化一个对象到 Guid 类型，此方法转化失败后不会抛异常
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Guid ToGuid(this object value)
    {
        return value.To<Guid>(default(Guid), false);
    }

    /// <summary>
    /// 转化一个对象到 bool 类型，此方法转化失败后不会抛异常
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool ToBool(this object value)
    {
        return value.To<bool>(default(bool), false);
    }

    /// <summary>
    /// 转化一个对象到 float 类型，此方法转化失败后不会抛异常
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float ToFloat(this object value)
    {
        return value.To<float>(default(float), false);
    }

    /// <summary>
    /// 转化一个对象到 double 类型，此方法转化失败后不会抛异常
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double ToDouble(this object value)
    {
        return value.To<double>(default(double), false);
    }
    #endregion

    #region [AsArray]
    /// <summary>
    /// 将一个数组转化类型成另外一个类型
    /// <para>此方法会忽略类型转化失败的元素</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static T[] AsArray<T>(this object[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            return default(T[]);
        }
        List<T> retArr = new List<T>();
        foreach (var item in arr)
        {
            try
            {
                var v = item.To<T>(default(T), false);
                retArr.Add(v);
            }
            catch { }
        }
        return retArr.ToArray();
    }
    #endregion
}