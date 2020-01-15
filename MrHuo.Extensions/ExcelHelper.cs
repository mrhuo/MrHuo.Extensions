using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace MrHuo.Extensions
{
    /// <summary>
    /// Excel 帮助类
    /// <para>包含功能：Excel 导入，Excel 导出</para>
    /// </summary>
    public static class ExcelHelper
    {
        #region [ICellExtensions]
        /// <summary>
        /// 获取单元格的数据
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static object GetCellValue(this ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.Formula:
                    return cell.CellFormula;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Blank:
                case CellType.String:
                case CellType.Unknown:
                default:
                    try
                    {
                        return cell.StringCellValue;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
            }
        }
        #endregion

        #region [Import]
        /// <summary>
        /// 从 Excel 导入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelFile">Excel 文件</param>
        /// <param name="columnsDef">列定义</param>
        /// <param name="includeTitleRow">是否包含标题行，默认包含</param>
        /// <param name="titleRowNum">标题行数，默认1行</param>
        /// <param name="sheetIndex">Sheet 索引</param>
        /// <returns></returns>
        public static List<T> Import<T>(
            string excelFile,
            List<(string PropertyName, int? ColumnIndex, Func<object, object> ValueProceed)> columnsDef,
            bool includeTitleRow = true,
            int titleRowNum = 1,
            int sheetIndex = 0)
            where T : new()
        {
            Ensure.FileExists(excelFile);
            Ensure.NotNull(columnsDef, nameof(columnsDef));
            Ensure.HasValue(columnsDef);

            IWorkbook workbook = null;
            FileStream hssfWorkbookFileStream = null;
            var fileExt = excelFile.GetFileExtensions();
            if (fileExt == ".xls")
            {
                hssfWorkbookFileStream = excelFile.GetReadStream();
                workbook = new HSSFWorkbook(hssfWorkbookFileStream);
            }
            else
            {
                workbook = new XSSFWorkbook(excelFile);
            }
            var sheetNums = workbook.NumberOfSheets;
            if (sheetNums == 0)
            {
                throw new Exception($"该 Excel 文件[{excelFile}]不存在任何 Sheet");
            }
            if (sheetIndex > sheetNums - 1)
            {
                throw new Exception($"该 Excel 文件共有 {sheetNums} 个 Sheet，不存在索引为 {sheetIndex} 的 Sheet");
            }
            var sheet = workbook.GetSheetAt(sheetIndex);
            var rowNums = sheet.PhysicalNumberOfRows - (includeTitleRow ? titleRowNum : 0);
            var startRowIndex = sheet.FirstRowNum + (includeTitleRow ? titleRowNum : 0);

            var ret = new List<T>();
            var properties = typeof(T).GetProperties();
            for (int rowNo = 0; rowNo < rowNums; rowNo++)
            {
                var row = sheet.GetRow(startRowIndex + rowNo);
                if (row == null)
                {
                    continue;
                }
                var cols = row.Cells;
                if (cols == null || cols.Count == 0)
                {
                    continue;
                }
                var dataItem = new T();
                var hasError = false;
                foreach (var def in columnsDef)
                {
                    var property = dataItem.GetProperty(def.PropertyName);
                    if (property == null)
                    {
                        continue;
                    }
                    try
                    {
                        object data = null;
                        if (def.ColumnIndex == null)
                        {
                            if (def.ValueProceed != null)
                            {
                                data = def.ValueProceed(data);
                            }
                        }
                        else
                        {
                            if (def.ColumnIndex.Value <= cols.Count - 1)
                            {
                                var col = cols[def.ColumnIndex.Value];
                                if (col != null)
                                {
                                    object value = col.GetCellValue();
                                    data = value.To(property.PropertyType);
                                    if (def.ValueProceed != null)
                                    {
                                        data = def.ValueProceed(data);
                                    }
                                }
                            }
                        }
                        dataItem.SetValue<T>(def.PropertyName, data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("EXCEPTION: " + ex.ToString());
                        hasError = true;
                    }
                }
                if (!hasError)
                {
                    ret.Add(dataItem);
                }
            }
            workbook.Close();
            if (hssfWorkbookFileStream != null)
            {
                hssfWorkbookFileStream.Close();
            }
            return ret;
        }
        #endregion

        #region [ExportDataTable]
        /// <summary>
        /// 导出 DataTable 到 Excel 文件
        /// </summary>
        /// <param name="dataTable">需要导出的 DataTable</param>
        /// <param name="sheetName">Sheet名称，默认 Sheet1</param>
        /// <param name="saveFile">Excel 文件路径，默认为空，创建一个临时文件</param>
        /// <param name="includeTitleRow">是否包含标题行，默认为true（包含）</param>
        /// <returns>导出的Excel文件绝对地址</returns>
        public static string ExportToFile(
            DataTable dataTable,
            string sheetName = null,
            string saveFile = null,
            bool includeTitleRow = true
        )
        {
            Ensure.NotNull(dataTable, nameof(dataTable));
            if (string.IsNullOrEmpty(saveFile))
            {
                saveFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n") + ".xls");
            }
            if (File.Exists(saveFile))
            {
                File.Delete(saveFile);
            }
            using (var stream = ExportToMemoryStream(dataTable, sheetName ?? "Sheet1", includeTitleRow))
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
        /// 导出 DataTable 到 MemoryStream
        /// </summary>
        /// <param name="dataTable">需要导出的 DataTable</param>
        /// <param name="sheetName">Sheet名称，默认 Sheet1</param>
        /// <param name="includeTitleRow">是否包含标题行，默认为true（包含）</param>
        /// <returns>MemoryStream</returns>
        public static MemoryStream ExportToMemoryStream(
            DataTable dataTable,
            string sheetName = null,
            bool includeTitleRow = true
        )
        {
            Ensure.NotNull(dataTable, nameof(dataTable));
            var workbook = new XSSFWorkbook();
            var sheet1 = workbook.CreateSheet(sheetName ?? "Sheet1");
            if (includeTitleRow)
            {
                //输出标题
                var titleRow = sheet1.CreateRow(0);
                var colIndex = 0;
                foreach (DataColumn item in dataTable.Columns)
                {
                    titleRow.CreateCell(colIndex++).SetCellValue(item.ColumnName);
                }
            }
            //输出内容
            var rowIndex = includeTitleRow ? 1 : 0;
            foreach (DataRow tableRow in dataTable.Rows)
            {
                var row = sheet1.CreateRow(rowIndex++);
                var colIndex = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    row.CreateCell(colIndex++).SetCellValue($"{tableRow[col]}");
                }
            }
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        #endregion

        #region [ExportIEnumerable]
        /// <summary>
        /// 导出 IEnumerable 到 Excel 文件
        /// </summary>
        /// <typeparam name="T">任意类型</typeparam>
        /// <param name="data">IEnumerable</param>
        /// <param name="columnDef">列定义，默认为 null，通过反射的方式确定列名</param>
        /// <param name="sheetName">Sheet名称，默认 Sheet1</param>
        /// <param name="saveFile">Excel 文件路径，默认为空，创建一个临时文件</param>
        /// <param name="includeTitleRow">是否包含标题行，默认为true（包含）</param>
        /// <returns>导出的Excel文件绝对地址</returns>
        public static string ExportToFile<T>(
            IEnumerable<T> data,
            Dictionary<string, Func<T, object>> columnDef = null,
            string sheetName = null,
            string saveFile = null,
            bool includeTitleRow = true
        )
        {
            Ensure.NotNull(data, nameof(data));
            if (string.IsNullOrEmpty(saveFile))
            {
                saveFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n") + ".xls");
            }
            if (File.Exists(saveFile))
            {
                File.Delete(saveFile);
            }
            var dir = Path.GetDirectoryName(saveFile);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            sheetName = sheetName ?? "Sheet1";
            using (var stream = ExportToMemoryStream(data, columnDef, sheetName, includeTitleRow))
            {
                using (FileStream fs = new FileStream(saveFile, FileMode.Create, FileAccess.Write))
                {
                    byte[] dataArray = stream.ToArray();
                    fs.Write(dataArray, 0, dataArray.Length);
                    fs.Flush();
                }
            }
            return saveFile;
        }

        /// <summary>
        /// 导出 IEnumerable 到 MemoryStream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">IEnumerable</param>
        /// <param name="columnDef">列定义，默认为 null，通过反射的方式确定列名</param>
        /// <param name="sheetName">Sheet名称，默认 Sheet1</param>
        /// <param name="includeTitleRow">是否包含标题行，默认为true（包含）</param>
        /// <returns></returns>
        public static MemoryStream ExportToMemoryStream<T>(
            IEnumerable<T> data,
            Dictionary<string, Func<T, object>> columnDef = null,
            string sheetName = null,
            bool includeTitleRow = true
        )
        {
            Ensure.NotNull(data, nameof(data));
            var workbook = new XSSFWorkbook();
            var sheet1 = workbook.CreateSheet(sheetName ?? "Sheet1");
            if (columnDef == null)
            {
                var properties = typeof(T).GetProperties();
                if (includeTitleRow)
                {
                    //输出标题
                    var titleRow = sheet1.CreateRow(0);
                    var colIndex = 0;
                    foreach (var item in properties)
                    {
                        titleRow.CreateCell(colIndex++).SetCellValue(item.Name);
                    }
                }
                //输出内容
                var rowIndex = includeTitleRow ? 1 : 0;
                foreach (var tableRow in data)
                {
                    var row = sheet1.CreateRow(rowIndex++);
                    var colIndex = 0;
                    foreach (var col in properties)
                    {
                        row.CreateCell(colIndex++).SetCellValue($"{col.GetValue(tableRow)}");
                        sheet1.AutoSizeColumn(colIndex);
                    }
                }
            }
            else
            {
                if (includeTitleRow)
                {
                    //输出标题
                    var titleRow = sheet1.CreateRow(0);
                    var colIndex = 0;
                    foreach (var item in columnDef)
                    {
                        titleRow.CreateCell(colIndex++).SetCellValue(item.Key);
                    }
                }

                //输出内容
                var rowIndex = includeTitleRow ? 1 : 0;
                foreach (var tableRow in data)
                {
                    var row = sheet1.CreateRow(rowIndex++);
                    var colIndex = 0;
                    foreach (var col in columnDef)
                    {
                        row.CreateCell(colIndex++).SetCellValue($"{col.Value(tableRow)}");
                        sheet1.AutoSizeColumn(colIndex);
                    }
                }
            }
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        #endregion
    }
}
