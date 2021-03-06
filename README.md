# MrHuo.Extensions

非常好用的扩展方法类库，大大提高开发效率和代码质量。同时适用于 .Net Framework 和 .Net Core

https://github.com/mrhuo/MrHuo.Extensions

尽量减少其他类库的依赖，目前仅依赖以下类库：

* Newtonsoft.Json 12.0.3
* NPOI 2.4.1

----

# Helper 类

#### ExcelHelper `Excel` 帮助类，支持 `.xls/.xlsx` 导入 `List<T>`，`DataTable/IEnumerable` 导出 `Excel`

#### Import 导入

```
//导入方法定义
List<T> Import<T>(
    //Excel 路径
    string excelFile,
    //类属性和 Excel 列 Mapping
    List<(string PropertyName, int? ColumnIndex, Func<object, object> ValueProceed)> columnsDef,
    //是否包含标题行，默认包含
    bool includeTitleRow = true,
    //标题行行数，默认1行标题
    int titleRowNum = 1,
    //需要导入的 Sheet 索引号，从 0 开始，默认 0
    int sheetIndex = 0
)
```
假如有如下 Excel 内容

|Name|Age|性别|
| ------ | ------ | ------ |
|aaa|111|男|
|bbb|222|女|
|ccc|333|男|
|4324234|23424|afwerwr|

类定义：

```
//故意定义的中英文混合，用于测试
class Student
{
    public int Index { get; set; }
    public string 姓名 { get; set; }
    public int 年龄 { get; set; }
    public string 性别 { get; set; }
    public DateTime 生日 { get; set; }
}
```

导入方法：

```
var excelFile = "E:\\test.xls";
var i = 0;
var ret = ExcelHelper.Import<Student>(excelFile, new List<(string PropertyName, int? ColumnIndex, Func<object, object> ValueProceed)>()
{
    //对应格式：属性名，Excel列索引号，自定义处理数据（null则取 Excel 单元格内容）
    //注意：如果类型转化错误，则设置为属性类型默认值
    //注意：列索引号为null时，Func<object, object> 参数值 data 为 null
    //这里自定义为 Index 属性赋值
    ("Index", null, data=> ++i),
    //姓名对应第0列
    ("姓名", 0, null),
    //年龄对应第1列
    ("年龄", 1, null),
    //性别对应第2列
    ("性别", 2, null),
    //生日使用当前时间作为值
    ("生日", null, data=> DateTime.Now),
    //这一列不会输出，也不会抛错，因为 Student 类不存在此属性，自动忽略
    ("123", null, data=> null)
});
```
//导出结果（我这里转化成了 JSON，更加直观一些）

```
[{
	"Index": 1,
	"姓名": "aaa",
	"年龄": 111,
	"性别": "男",
	"生日": "2020-01-07T04:53:26.0283775+08:00"
}, {
	"Index": 2,
	"姓名": "bbb",
	"年龄": 222,
	"性别": "女",
	"生日": "2020-01-07T04:53:26.0288609+08:00"
}, {
	"Index": 3,
	"姓名": "ccc",
	"年龄": 333,
	"性别": "男",
	"生日": "2020-01-07T04:53:26.0288754+08:00"
}, {
	"Index": 4,
	"姓名": "4324234",
	"年龄": 23424,
	"性别": "afwerwr",
	"生日": "2020-01-07T04:53:26.0289049+08:00"
}]
```

----

## String 扩展方法

* `Count(string subString, bool ignoreCase = false)` - 统计字符串中子字符串的个数
* `Count(char character, bool ignoreCase = false)` - 统计字符串中某个字符的个数
* `Repeat(int repeatTimes)` - 将字符串重复指定次数
* `Capitalize()` - 把字符串的第一个字符大写
* `Capitalize(int len)` - 把字符串的前n个字符大写
* `Capitalize(int start, int len)` - 把字符串中的某几个字符大写
* `IsUpper()` - 验证一个字符串中所有字符是否全部大写
* `IsLower()` - 验证一个字符串中所有字符是否全部小写
* `IsNumeric()` - 验证一个字符串是否为数字（可带小数点）
* `IsInt()` - 验证一个字符串是否为整形（不带小数点）
* `IsMobile()` - 验证一个字符串是否是手机号码
* `IsEmail()` - 验证一个字符串是否是电子邮件地址
* `IsUrl(bool includeProtocal = true)` - 验证一个字符串是否为URL地址
* `PadCenter(int width)` - 返回一个原字符串居中,并使用空格填充至长度 width 的新字符串
* `PadCenter(char paddingChar, int width)` - 返回一个原字符串居中,并使用指定填充字符串填充至长度 width 的新字符串
* `Reverse()` - 反转字符串
* `Left(int len)` - 截取字符串左边 len 个字符，和 SubString 不同，此方法不抛异常
* `Right(int len)` - 截取字符串右边 len 个字符，和 SubString 不同，此方法不抛异常
* `SplitLines()` - 将字符串分割为行（分隔符 \n，\r\n）
* `Remove(string subString)` - 移除字符串中的指定字符串，返回新的字符串
* `To<TType>(TType defaultValue = default(TType))` - 字符串类型转化到任意值类型，失败时返回值类型默认值
* `ToBase64(Encoding encoding = null)` - 字符串 Base64 加密，默认 UTF-8 编码
* `FromBase64(Encoding encoding = null)` - 字符串 Base64 解密，默认 UTF-8 编码
* `ToBytes(Encoding encoding = null)` - 字符串转换到字节，默认 UTF-8 编码
* `ToHex()` - 字符串转化成16进制字符串
* `HexStringToHexBytes()` - 16进制的字符串转化到16进制的字节数组
* `HexStringToString(Encoding encoding = null)` - 16进制的字符串解码为字符串
* `DESEncrypt(byte[] key, byte[] iv)` - DES 加密
* `DESEncrypt(string key, byte[] iv)` - DES 加密，密码为字符串
* `DESEncrypt(string key)` - DES 加密，密码为字符串，加密向量默认为本类的字节数组 Encoding.UTF8.GetBytes($"MrHuo.Extensions")
* `DESDecrypt(byte[] key, byte[] iv)` - DES 解密
* `DESDecrypt(string key, byte[] iv)` - DES 解密，密码为字符串
* `DESDecrypt(string key)` - DES 解密，密码为字符串，加密向量默认为本类的全名
* `ToMd5(Encoding encoding = null)` - MD5 加密，默认 UTF-8 编码
* `HtmlEncode()` - 字符串HTML编码
* `HtmlDecode()` - 字符串HTML解码
* `UrlEncode(Encoding encoding = null)` - 字符串URL编码，默认 UTF-8 编码
* `UrlDecode(Encoding encoding = null)` - 字符串URL解码，默认 UTF-8 编码
* `ParseQueryString(Encoding encoding = null)` - QueryString 字符串转化成字典，默认 UTF-8 编码
* `Join(string separator = "")` - 字符串数组 JOIN 成一个字符串，默认分隔符为 string.Empty
* `CleanHtml()` - 清除字符串中的 HTML 标记
* `SubStringEx(int start, int length)` - 扩展的 SubString，不会抛异常，如果超出范围，返回空字符串
* `ToFile(string fileName, Encoding encoding = null)` - 将一个字符串写入到文件
* `ToFileMd5()` - 获取文件的 MD5

```
//获取文件的 MD5
var fileMd5 = "C:\\a.txt".ToFileMd5();
```

* `ToStream()` - 将一个文件读取到 MemoryStream
* `AppendToFile(string fileName, Encoding encoding = null)` - 字符串追加到文件

```
//追加文件
var fileName = Path.GetTempFileName();
DateTime.Now.ToString("yyyyMMddHHmmss").AppendToFile(fileName);
DateTime.Now.ToString("yyyyMMddHHmmss").AppendToFile(fileName);
DateTime.Now.ToString("yyyyMMddHHmmss").AppendToFile(fileName);
DateTime.Now.ToString("yyyyMMddHHmmss").AppendToFile(fileName);
```

* `CopyFileTo(string toFileName, bool overwrite = true)` - 文件复制（注意：默认 overwrite 为 true，目标文件存在会被覆盖）
* `MoveFileTo(string toFileName)` - 文件移动
* `HttpGet(bool throwException = false)` - GET 请求 URL，如果发生错误，返回 null

```
//获取网址 https://www.github.com 的HTML
var html = "https://www.github.com".HttpGet();
```

* `HttpGet<T>(bool throwException = false)` - GET 请求 URL，反序列化为对象

```
//获取网址HTML，并序列化为指定的对象
class RestResult
{
	public int Code { get; set; }
	public string Msg { get; set; }
}
var restResult = "https://www.github.com".HttpGet<RestResult>();
```

* `HttpPost(Dictionary<string, object> data = null, bool throwException = false)` - POST 请求 URL

```
//POST 请求
var html = "http://www.example.com".HttpPost(new Dictionary<string, object>()
{
    ["userId"] = "xxx",
    ["userName"] = "xxx"
});
var restResult = "http://www.example.com".HttpPost<RestResult>(new Dictionary<string, object>()
{
    ["userId"] = "xxx",
    ["userName"] = "xxx"
});
```

## IEnumerable 扩展方法

* `ToDataTable<T>(Dictionary<string, Func<T, object>> columnDef = null, string tableName = null)` - IEnumerable 对象转换到 DataTable

```
class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var list = new List<Student>();
//list 转化为 DataTable，默认列为 Student 对象属性
var dt = list.ToDataTable(); 
var index = 1;
//list 转化为 DataTable，使用列定义
var dt2 = list.ToDataTable(new Dictionary<string, Func<Student, object>>()
{
    ["顺序"] = (item) => $"{index++}",
    ["姓名"] = (item) => item.Name,
    ["年龄"] = (item) => item.Age,
    ["是否成年"] = (item) => item.Age > 16 ? "是" : "否"
});
```

* `ToExcelFile<T>(Dictionary<string, Func<T, object>> columnDef = null, string sheetName = "Sheet1", string saveFile = null, bool includeTitleRow = true)` - IEnumerable 对象导出到 Excel 文件
* `ToExcelStream<T>(Dictionary<string, Func<T, object>> columnDef = null, string sheetName = "Sheet1", bool includeTitleRow = true)` - IEnumerable 对象输出到 Excel 内存流

## DateTime 扩展方法

* `ToUnixTime()` - 时间转化为Unix时间戳
* `ToDateTime()` - Unix时间戳转化为C#时间格式
* `Format(string formatStr = "yyyy-MM-dd HH:mm:ss")` - 格式化日期

## ExceptionExtensions 扩展方法

* `GetTopException()` - 获取最顶层的异常

## ICollection 扩展方法

* `HasValue<T>()` - 验证一个集合是否有元素

## NameValueCollection 扩展方法

* `ToDictionary()` - 转化成字典

## ICustomAttributeProvider 扩展方法（反射常用扩展方法）

* `HasAttribute<TAttribute>(bool inherit = false)` - 实例是否具有 TAttribute 类型的特性
* `GetAttributes<TAttribute>(bool inherit = false)` - 获取实例的所有 TAttribute 类型的特性
* `GetAttributes<TAttribute>(bool inherit = false)` - 获取实例的第一个 TAttribute 类型的特性
* `EveryPropertyInvoke<T>(Action<PropertyInfo, Object> propertyAction)` - 对象实例的每一个公开可读属性调用委托处理

## Byte 扩展方法

* `AppendToFile(string fileName)` - 将字节数组追加到文件，如果文件不存在，文件自动创建
* `ToFile(string fileName)` - 将字节数组写入到文件，如果文件不存在，文件自动创建
* `ToHex()` - 转化成16进制的字符串
* `ToStringEx(encoding = null)` - 转化成字符串，默认 UTF8 编码

## ObjectExtensions 扩展方法

* `FromJson<T>(JsonSerializerSettings settings = null)` - 将一个 JSON 字符串反序列化成对象
* `ToJson(Formatting formatting = 0, JsonSerializerSettings settings = null)` - 将任意类型对象序列化成 JSON


----
更多文档，持续更新中....

可能的计划：

- [ ] ImageHelper，图片各种处理。可使用方法：

```
"E:\\aa.jpg".MakeThumbnail(50);
"E:\\aa.jpg".AddImageMark("E:\\bb.jpg", BOTTOM_RIGHT);
```

- [ ] EmailHelper，发送电子邮件。可使用方法：

```
EmailHelper.InitSmtp(server, port, account, pwd);
"admin@mrhuo.com".SendEmail("title", "content");
```

- [ ] 导出 Excel 时可通过自定义模板导出数据
- [ ] ?? 导出Word，导出Pdf
- [ ] 