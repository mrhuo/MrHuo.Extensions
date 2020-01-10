using MrHuo.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

/// <summary>
/// IEnumerable 扩展方法
/// </summary>
public static class IEnumerableExtensions
{
    #region [ToDataTable]
    /// <summary>
    /// IEnumerable 对象转换到 DataTable
    /// </summary>
    /// <typeparam name="T">任意类型</typeparam>
    /// <param name="data">IEnumerable 对象</param>
    /// <param name="columnDef">列定义，默认为 null，通过反射的方式确定列名</param>
    /// <param name="tableName">表名，默认为null，取 nameof(T)</param>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(this IEnumerable<T> data, Dictionary<string, Func<T, object>> columnDef = null, string tableName = null)
    {
        return DataTableHelper.IEnumerableToDataTable(data, columnDef, tableName);
    }
    #endregion

    #region [ToExcel]
    /// <summary>
    /// IEnumerable 对象导出到 Excel 文件
    /// </summary>
    /// <typeparam name="T">任意类型</typeparam>
    /// <param name="data"></param>
    /// <param name="columnDef">列定义，默认为 null，通过反射的方式确定列名</param>
    /// <param name="sheetName">Sheet名称，默认 Sheet1</param>
    /// <param name="saveFile">Excel 文件路径，默认为空，创建一个临时文件</param>
    /// <param name="includeTitleRow">是否包含标题行，默认为true（包含）</param>
    /// <returns></returns>
    public static string ToExcelFile<T>(
        this IEnumerable<T> data,
        Dictionary<string, Func<T, object>> columnDef = null,
        string sheetName = "Sheet1",
        string saveFile = null,
        bool includeTitleRow = true)
    {
        return ExcelHelper.ExportToFile(data, columnDef, sheetName, saveFile, includeTitleRow);
    }

    /// <summary>
    /// IEnumerable 对象输出到 Excel 内存流
    /// </summary>
    /// <typeparam name="T">任意类型</typeparam>
    /// <param name="data">IEnumerable</param>
    /// <param name="columnDef">列定义，默认为 null，通过反射的方式确定列名</param>
    /// <param name="sheetName">Sheet名称，默认 Sheet1</param>
    /// <param name="includeTitleRow">是否包含标题行，默认为true（包含）</param>
    /// <returns></returns>
    public static MemoryStream ToExcelStream<T>(
        this IEnumerable<T> data,
        Dictionary<string, Func<T, object>> columnDef = null,
        string sheetName = "Sheet1",
        bool includeTitleRow = true)
    {
        return ExcelHelper.ExportToMemoryStream(data, columnDef, sheetName, includeTitleRow);
    }
    #endregion

    #region [Random]
    /// <summary>
    /// 随机排序后选择前 take 个元素，为 0 时返回全部
    /// </summary>
    /// <typeparam name="T">任意类型</typeparam>
    /// <param name="data"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public static IEnumerable<T> Random<T>(this IEnumerable<T> data, int take = 0)
    {
        return RandomHelper.RandomSome(data, take);
    }

    /// <summary>
    /// 随机返回一个元素
    /// </summary>
    /// <typeparam name="T">任意类型</typeparam>
    /// <param name="data"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public static T Random<T>(this IEnumerable<T> data)
    {
        return RandomHelper.RandomOne(data);
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
    public static T[] AsArray<T>(this IEnumerable<object> arr)
    {
        if (arr == null || arr.Count() == 0)
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