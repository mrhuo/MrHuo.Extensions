using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Exception 类扩展方法
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// 获取最顶层的异常信息
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static Exception GetTopException(this Exception ex)
    {
        if (ex == null)
        {
            return ex;
        }
        if (ex.InnerException == null)
        {
            return ex;
        }
        return GetTopException(ex.InnerException);
    }
}