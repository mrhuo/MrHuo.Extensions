using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MrHuo.Extensions
{
    /// <summary>
    /// DataTable 帮助类
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// 将一个 IEnumerable 对象转换到 DataTable
        /// </summary>
        /// <typeparam name="T">任意类型</typeparam>
        /// <param name="data">IEnumerable 对象</param>
        /// <param name="columnDef">列定义，默认为 null，通过反射的方式确定列名</param>
        /// <param name="tableName">表名，默认为null，取 nameof(T)</param>
        /// <returns></returns>
        public static DataTable IEnumerableToDataTable<T>(IEnumerable<T> data, Dictionary<string, Func<T, object>> columnDef = null, string tableName = null)
        {
            Ensure.NotNull(data);
            var dataTable = new DataTable(tableName ?? typeof(T).Name);
            if (columnDef == null)
            {
                var properties = typeof(T).GetProperties();
                foreach (var item in properties)
                {
                    dataTable.Columns.Add(new DataColumn(item.Name));
                }
                foreach (var row in data)
                {
                    var dr = dataTable.NewRow();
                    foreach (var col in properties)
                    {
                        dr[col.Name] = col.GetValue(row);
                    }
                    dataTable.Rows.Add(dr);
                }
            }
            else
            {
                foreach (var item in columnDef)
                {
                    dataTable.Columns.Add(new DataColumn(item.Key));
                }
                foreach (var row in data)
                {
                    var dr = dataTable.NewRow();
                    foreach (var col in columnDef)
                    {
                        dr[col.Key] = col.Value(row);
                    }
                    dataTable.Rows.Add(dr);
                }
            }
            return dataTable;
        }
    }
}
