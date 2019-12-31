using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
/// IEnumerable 扩展方法
/// </summary>
public static class IEnumerableExtensions
{
    #region [ToDataTable]
    /// <summary>
    /// 将一个 IEnumerable 对象转换到 DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <param name="tableName"></param>
    /// <param name="columnDef"></param>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(this IEnumerable<T> table, string tableName, Dictionary<string, Func<T, object>> columnDef)
    {
        var dataTable = new DataTable(tableName);
        foreach (var item in columnDef)
        {
            dataTable.Columns.Add(new DataColumn(item.Key));
        }
        foreach (var row in table)
        {
            var dr = dataTable.NewRow();
            foreach (var col in columnDef)
            {
                dr[col.Key] = col.Value(row);
            }
            dataTable.Rows.Add(dr);
        }
        return dataTable;
    }
    /// <summary>
    /// 将一个 IEnumerable 对象转换到 DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <param name="columnDef"></param>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(this IEnumerable<T> table, Dictionary<string, Func<T, object>> columnDef)
    {
        return ToDataTable(table, typeof(T).Name, columnDef);
    }
    #endregion

    #region [ToExcel]
    /// <summary>
    /// 导出到 Excel 文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <param name="columnDef"></param>
    /// <param name="sheetName"></param>
    /// <param name="saveFile"></param>
    /// <returns></returns>
    public static string ToExcelFile<T>(
        this IEnumerable<T> table,
        Dictionary<string, Func<T, object>> columnDef,
        string sheetName = "Sheet1",
        string saveFile = null)
    {
        if (string.IsNullOrEmpty(saveFile))
        {
            saveFile = Path.GetTempFileName();
        }
        if (!saveFile.EndsWith(".xls"))
        {
            saveFile += ".xls";
        }
        if (File.Exists(saveFile))
        {
            File.Delete(saveFile);
        }
        using (var stream = table.ToExcelStream(columnDef, sheetName))
        {
            using (FileStream fs = new FileStream(saveFile, FileMode.Create, FileAccess.Write))
            {
                byte[] data = stream.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
        }
        return saveFile;
    }

    /// <summary>
    /// 将一个 IEnumerable 对象输出到 Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <param name="columnDef">列定义</param>
    /// <returns></returns>
    public static MemoryStream ToExcelStream<T>(
        this IEnumerable<T> table, 
        Dictionary<string, Func<T, object>> columnDef,
        string sheetName = "Sheet1")
    {
        var workbook = new XSSFWorkbook();
        var sheet1 = workbook.CreateSheet(sheetName);
        //输出标题
        var titleRow = sheet1.CreateRow(0);
        var colIndex = 0;
        foreach (var item in columnDef)
        {
            titleRow.CreateCell(colIndex++).SetCellValue(item.Key);
        }
        //输出内容
        var rowIndex = 1;
        foreach (var tableRow in table)
        {
            var row = sheet1.CreateRow(rowIndex ++);
            colIndex = 0;
            foreach (var col in columnDef)
            {
                row.CreateCell(colIndex++).SetCellValue($"{col.Value(tableRow)}");
            }
        }
        var ms = new MemoryStream();
        workbook.Write(ms);
        ms.Flush();
        return ms;
    }
    #endregion
}