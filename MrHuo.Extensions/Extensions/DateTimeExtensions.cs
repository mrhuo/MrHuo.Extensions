using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// DateTime 扩展方法
/// </summary>
public static class DateTimeExtensions
{
    #region [ToUnixTime]
    /// <summary>
    /// 将时间转化为Unix时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToUnixTime(this DateTime dateTime)
    {
        TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return (long)(ts.TotalSeconds);
    }
    #endregion

    #region [ToDateTime]
    /// <summary>
    /// Unix时间戳转化为C#时间格式
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this long unixTime)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(unixTime + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }
    #endregion

    #region [Format]
    /// <summary>
    /// 格式化日期
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="formatStr"></param>
    /// <returns></returns>
    public static string Format(this DateTime dateTime, string formatStr = "yyyy-MM-dd HH:mm:ss")
    {
        return dateTime.ToString(formatStr);
    }
    #endregion
}