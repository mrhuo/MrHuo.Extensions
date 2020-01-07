using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MrHuo.Extensions.Test
{
    public class CsvHelperTest
    {
        [Test]
        public void TestCsvImport()
        {
            var csvFile = "E:\\test.csv";
            var i = 0;
            var list = CsvHelper.Import<Student>(csvFile, new List<(string PropertyName, int? ColumnIndex, Func<object, object> ValueProceed)>()
            {
                ("Index", null, data=> ++i),
                ("姓名", 0, null),
                ("年龄", 1, null),
                ("性别", 2, null),
                ("生日", 3, null)
            });
        }
    }

    class Student
    {
        public int Index { get; set; }
        public string 姓名 { get; set; }
        public int 年龄 { get; set; }
        public string 性别 { get; set; }
        public DateTime 生日 { get; set; }
    }
}
