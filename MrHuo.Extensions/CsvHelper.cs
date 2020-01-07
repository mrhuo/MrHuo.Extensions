using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.Extensions
{
    /// <summary>
    /// CSV 帮助类
    /// </summary>
    public static class CsvHelper
    {
        /// <summary>
        /// 从 CSV 文件中导入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvFile"></param>
        /// <param name="columnsDef"></param>
        /// <param name="includeTitleRow"></param>
        /// <param name="titleRowNum"></param>
        /// <returns></returns>
        public static List<T> Import<T>(
            string csvFile,
            List<(string PropertyName, int? ColumnIndex, Func<object, object> ValueProceed)> columnsDef,
            bool includeTitleRow = true,
            int titleRowNum = 1)
            where T : new()
        {
            Ensure.NotNull(csvFile);
            Ensure.FileExists(csvFile);
            Ensure.NotNull(columnsDef);
            var ret = new List<T>();
            var lines = csvFile.ReadFileAllLines();
            if (lines.Count == 0)
            {
                return ret;
            }
            var firstLine = lines.FirstOrDefault();
            var maxColumns = firstLine.Split(',').Length;

            var startLine = 0;
            if (includeTitleRow && titleRowNum > 0)
            {
                startLine = startLine + titleRowNum;
            }
            foreach (var line in lines.Skip(startLine))
            {
                var lineData = line.Split(',');
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
                            if (def.ColumnIndex.Value <= maxColumns - 1)
                            {
                                var value = lineData[def.ColumnIndex.Value];
                                if (!string.IsNullOrEmpty(value))
                                {
                                    data = ((object)value).To(property.PropertyType);
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
            return ret;
        }

        //TODO: 
        //ExportDataTable()
        //ExportIEnumerable()
        //ExportJsonFile()
    }
}
