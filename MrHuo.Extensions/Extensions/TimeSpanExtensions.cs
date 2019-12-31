using System;
using System.Collections.Generic;
using System.Text;

public static class TimeSpanExtensions
{
    public static string FormatTime(this TimeSpan timeSpan)
    {
        var ms = timeSpan.TotalMilliseconds;
        if (ms < 1000)
        {
            return timeSpan.TotalMilliseconds.ToString("F2") + "毫秒";
        }
        if (ms < 1000 * 60)
        {
            return timeSpan.TotalSeconds.ToString("F2") + "秒";
        }
        if (ms < 1000 * 60 * 60)
        {
            return timeSpan.TotalMinutes.ToString("F2") + "分钟";
        }
        return timeSpan.TotalHours.ToString("F2") + "小时";
    }
}