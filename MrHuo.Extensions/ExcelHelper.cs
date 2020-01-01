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
