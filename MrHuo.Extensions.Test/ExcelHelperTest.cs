using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class TransferTask
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsOk { get; set; }
        public string TransferDetail { get; set; }
    }
    public class ExcelHelperTest
    {
        private List<TransferTask> GetTransferTasks()
        {
            var tasks = new List<TransferTask>();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(new TransferTask()
                {
                    CreateTime = DateTime.Now,
                    IsOk = i % 5 == 0,
                    From = "FromApi_" + i,
                    To = "ToApi_" + i,
                    TaskId = Guid.NewGuid().ToString("n"),
                    TaskName = "Task_" + (i + 1),
                    TransferDetail = $"Transfer [{"FromApi_" + i}] To [{"ToApi_" + i}] {(i % 5 == 0 ? "成功" : "失败")}"
                });
            }
            return tasks;
        }

        [Test]
        public void TestExportDictionary()
        {
            var transferTaskDict = GetTransferTasks().ToDictionary(p => p.TaskId, p => p);
            var fileName = transferTaskDict.ToExcelFile(new Dictionary<string, Func<KeyValuePair<string, TransferTask>, object>>()
            {
                ["时间"] = (item) => item.Value.CreateTime.Format(),
                ["任务名"] = (item) => item.Value.TaskName,
                ["任务Id"] = (item) => item.Key,
                ["是否成功"] = (item) => item.Value.IsOk ? "成功" : "失败",
                ["FROM"] = (item) => item.Value.From,
                ["TO"] = (item) => item.Value.To,
                ["详情"] = (item) => item.Value.TransferDetail
            });
            Assert.Pass($"TestExportDictionary: 已导出到【{fileName}】");
        }

        [Test]
        public void TestExportDateTable()
        {
            var transferTaskDataTable = GetTransferTasks().ToDataTable();
            var fileName = MrHuo.Extensions.ExcelHelper.ExportToFile(transferTaskDataTable);
            Assert.Pass($"TestExportDateTable: 已导出到【{fileName}】");
        }

        [Test]
        public void TestExportDateTable2()
        {
            var index = 1;
            var transferTaskDataTable = GetTransferTasks().ToDataTable(new Dictionary<string, Func<TransferTask, object>>()
            {
                ["Id"] = (item) => $"{index++}",
                ["时间"] = (item) => item.CreateTime.Format(),
                ["任务名"] = (item) => item.TaskName,
                ["任务Id"] = (item) => item.TaskId,
                ["是否成功"] = (item) => item.IsOk ? "成功" : "失败",
                ["FROM"] = (item) => item.From,
                ["TO"] = (item) => item.To,
                ["详情"] = (item) => item.TransferDetail
            });
            var fileName = MrHuo.Extensions.ExcelHelper.ExportToFile(transferTaskDataTable);
            Assert.Pass($"TestExportDateTable: 已导出到【{fileName}】");
        }
    }
}
