using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tests
{
    public class IEnumerableExtensionsTest
    {
        class StudentModel
        {
            public string UserName { get; set; }
            public string UserPassword { get; set; }
            public int UserAge { get; set; }
        }

        [Test]
        public void TestToExcelFile()
        {
            var testData = new List<StudentModel>();
            for (int i = 0; i < 12; i++)
            {
                testData.Add(new StudentModel()
                {
                    UserAge = i * 2,
                    UserName = "Name" + i,
                    UserPassword = "Password" + i
                });
            }
            var file = testData.ToExcelFile(new Dictionary<string, Func<StudentModel, object>>()
            {
                ["年龄"] = (m) => m.UserAge,
                ["名字"] = (m) => m.UserName,
                ["密码"] = (m) => m.UserPassword
            });
            Assert.Pass(file);
        }
    }
}
