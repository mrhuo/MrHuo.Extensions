# MrHuo.Extensions

非常好用的扩展方法类库，大大提高开发效率和代码质量。同时适用于 .Net Framework 和 .Net Core

https://github.com/mrhuo/MrHuo.Extensions

尽量减少其他类库的依赖，目前仅依赖以下类库：

* Newtonsoft.Json 12.0.3
* NPOI 2.4.1

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
* `To&lt;TType&gt;(TType defaultValue = default(TType))` - 字符串类型转化到任意值类型，失败时返回值类型默认值
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

## IEnumerable 扩展方法

* `ToDataTable&lt;T&gt;(Dictionary&lt;string, Func&lt;T, object&gt;&gt; columnDef = null, string tableName = null)` - IEnumerable 对象转换到 DataTable

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

* `ToExcelFile&lt;T&gt;(Dictionary&lt;string, Func&lt;T, object&gt;&gt; columnDef = null, string sheetName = "Sheet1", string saveFile = null, bool includeTitleRow = true)` - IEnumerable 对象导出到 Excel 文件
* `ToExcelStream&lt;T&gt;(Dictionary&lt;string, Func&lt;T, object&gt;&gt; columnDef = null, string sheetName = "Sheet1", bool includeTitleRow = true)` - IEnumerable 对象输出到 Excel 内存流

## DateTime 扩展方法

* `ToUnixTime()` - 时间转化为Unix时间戳
* `ToDateTime()` - Unix时间戳转化为C#时间格式
* `Format(string formatStr = "yyyy-MM-dd HH:mm:ss")` - 格式化日期

## ExceptionExtensions 扩展方法

* `GetTopException()` - 获取最顶层的异常

## ICollection 扩展方法

* `HasValue&lt;T&gt;()` - 验证一个集合是否有元素

## NameValueCollection 扩展方法

* `ToDictionary()` - 转化成字典

## ICustomAttributeProvider 扩展方法（反射常用扩展方法）

* `HasAttribute&lt;TAttribute&gt;(bool inherit = false)` - 实例是否具有 TAttribute 类型的特性
* `GetAttributes&lt;TAttribute&gt;(bool inherit = false)` - 获取实例的所有 TAttribute 类型的特性
* `GetAttributes&lt;TAttribute&gt;(bool inherit = false)` - 获取实例的第一个 TAttribute 类型的特性
* `EveryPropertyInvoke&lt;T&gt;(Action&lt;PropertyInfo, Object&gt; propertyAction)` - 对象实例的每一个公开可读属性调用委托处理


----
更多文档，持续更新中....