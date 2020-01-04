using MrHuo.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// String 扩展方法
/// </summary>
public static class StringExtensions
{
    private static readonly Encoding DefaultEncoding = Encoding.UTF8;

    #region [Count]
    /// <summary>
    /// 统计字符串中的某个子字符串的个数
    /// <para>字符串或子字符串为空返回0</para>
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="subString">子字符串</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns></returns>
    public static int Count(this string str, string subString, bool ignoreCase = false)
    {
        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(subString) || subString.Length > str.Length)
        {
            return 0;
        }
        if (ignoreCase)
        {
            str = str.ToLower();
            subString = subString.ToLower();
        }
        var tmp = str;
        var count = 0;
        while (subString.Length <= tmp.Length)
        {
            if (tmp.StartsWith(subString))
            {
                count++;
            }
            tmp = tmp.Substring(1);
        }
        return count;
    }

    /// <summary>
    /// 统计字符串中的某个字符的个数
    /// <para>字符串或子字符串为空返回0</para>
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="character">字符</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns></returns>
    public static int Count(this string str, char character, bool ignoreCase = false)
    {
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        var tmpChar = character;
        if (ignoreCase)
        {
            str = str.ToLower();
            if (tmpChar >= 65 && tmpChar <= 96)
            {
                tmpChar = (char)(tmpChar + 32);
            }
        }
        var tmp = str;
        var count = 0;
        while (tmp.Length > 0)
        {
            if (tmp.IndexOf(tmpChar) == 0)
            {
                count++;
            }
            tmp = tmp.Substring(1);
        }
        return count;
    }
    #endregion

    #region [Repeat]
    /// <summary>
    /// 将字符串重复指定次数
    /// <para>字符串为空或重复次数小于2则返回原字符串</para>
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="repeatTimes">重复次数</param>
    /// <returns></returns>
    public static string Repeat(this string str, int repeatTimes)
    {
        if (string.IsNullOrEmpty(str) || repeatTimes <= 1)
        {
            return str;
        }
        var ret = "";
        for (int i = 0; i < repeatTimes; i++)
        {
            ret += str;
        }
        return ret;
    }
    #endregion

    #region [Capitalize]
    /// <summary>
    /// 把字符串的第一个字符大写
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Capitalize(this string str)
    {
        return str.Substring(0, 1).ToUpper() + str.Substring(1);
    }

    /// <summary>
    /// 把字符串的前n个字符大写
    /// </summary>
    /// <param name="str"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public static string Capitalize(this string str, int len)
    {
        return str.Substring(0, len).ToUpper() + str.Substring(len);
    }

    /// <summary>
    /// 把字符串中的某几个字符大写
    /// </summary>
    /// <param name="str"></param>
    /// <param name="start"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public static string Capitalize(this string str, int start, int len)
    {
        if (start == 0)
        {
            return str.Capitalize(len);
        }
        return str.Substring(0, start) + str.Substring(start, len).ToUpper() + str.Substring(start + len);
    }
    #endregion

    #region [IsUpper]
    /// <summary>
    /// 验证一个字符串所有字符是否全部大写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static bool IsUpper(this string str)
    {
        var tmp = str;
        var count = 0;
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i].IsUpper())
            {
                count++;
            }
        }
        return count == tmp.Length;
    }
    #endregion

    #region [IsLower]
    /// <summary>
    /// 验证一个字符串所有字符是否全部小写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static bool IsLower(this string str)
    {
        var tmp = str;
        var count = 0;
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i].IsLower())
            {
                count++;
            }
        }
        return count == tmp.Length;
    }
    #endregion

    #region [IsNumeric]
    /// <summary>
    /// 验证一个字符串是否为数字（可带小数点）
    /// <param name="value"></param>
    /// </summary>
    public static bool IsNumeric(this string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
    }
    #endregion

    #region [IsInt]
    /// <summary>
    /// 验证一个字符串是否为整形（不带小数点）
    /// <param name="value"></param>
    /// </summary>
    public static bool IsInt(this string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*$");
    }
    #endregion

    #region [IsMobile]
    /// <summary>
    /// 验证一个字符串是否是手机号码
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsMobile(this string value)
    {
        return Regex.IsMatch(value, @"^1[345789][0-9]{9}$");
    }
    #endregion

    #region [IsEmail]
    /// <summary>
    /// 验证一个字符串是否是电子邮件地址
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsEmail(this string value)
    {
        return Regex.IsMatch(value, @"^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$");
    }
    #endregion

    #region [IsUrl]
    /// <summary>
    /// 验证一个字符串是否为URL地址
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsUrl(this string value, bool includeProtocal = true)
    {
        return Regex.IsMatch(value, @"^((http|ftp|https):\/\/)" + (!includeProtocal ? "?" : "") + @"[\w]+(.[\w]+)([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])$");
    }
    #endregion

    #region [PadCenter]
    /// <summary>
    /// 返回一个原字符串居中,并使用空格填充至长度 width 的新字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="width">总长度</param>
    /// <returns></returns>
    public static string PadCenter(this string str, int width)
    {
        return str.PadCenter(' ', width);
    }

    /// <summary>
    /// 返回一个原字符串居中,并使用指定填充字符串填充至长度 width 的新字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="paddingChar">填充字符</param>
    /// <param name="width">总长度</param>
    /// <returns></returns>
    public static string PadCenter(this string str, char paddingChar, int width)
    {
        if (string.IsNullOrEmpty(str))
        {
            return $"{paddingChar}".Repeat(width);
        }
        if (width < str.Length)
        {
            return str;
        }
        var space = width - str.Length;
        if (space % 2 == 0)
        {
            return $"{paddingChar}".Repeat(space / 2) + str + $"{paddingChar}".Repeat(space / 2);
        }
        return $"{paddingChar}".Repeat(space / 2) + str + $"{paddingChar}".Repeat((space + 1) / 2);
    }
    #endregion

    #region [Reverse]
    /// <summary>
    /// 反转字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static string Reverse(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }
        return string.Join("", str.ToCharArray().Reverse());
    }
    #endregion

    #region [Left]
    /// <summary>
    /// 截取字符串左边 len 个字符，和 SubString 不同，此方法不抛异常
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="len">长度</param>
    /// <returns></returns>
    public static string Left(this string str, int len)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }
        if (len > str.Length || len < 1)
        {
            return str;
        }
        return str.Substring(0, len);
    }
    #endregion

    #region [Right]
    /// <summary>
    /// 截取字符串右边 len 个字符，和 SubString 不同，此方法不抛异常
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="len">长度</param>
    /// <returns></returns>
    public static string Right(this string str, int len)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }
        if (len > str.Length || len < 1)
        {
            return str;
        }
        return str.Reverse().Substring(0, len).Reverse();
    }
    #endregion

    #region [SplitLines]
    /// <summary>
    /// 包含各行作为元素的数组
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string[] SplitLines(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return new string[] { };
        }
        var tmp = str.Replace("\r", "");
        return tmp.Split('\n');
    }
    #endregion

    #region [Remove]
    /// <summary>
    /// 移除字符串中的指定字符串，返回新的字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="subString">需要移除的指定字符串</param>
    /// <returns></returns>
    public static string Remove(this string str, string subString)
    {
        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(subString) || subString.Length > str.Length)
        {
            return str;
        }
        return str.Replace(subString, string.Empty);
    }
    #endregion

    #region [To]
    /// <summary>
    /// 字符串类型转化到任意值类型，失败时返回值类型默认值
    /// </summary>
    /// <typeparam name="TType">任意值类型</typeparam>
    /// <param name="str">字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static TType To<TType>(this string str, TType defaultValue = default(TType))
        where TType : struct
    {
        if (string.IsNullOrEmpty(str))
        {
            return defaultValue;
        }
        try
        {
            return (TType)Convert.ChangeType(str, typeof(TType));
        }
        catch
        {
            return defaultValue;
        }
    }
    #endregion

    #region [ToBase64]
    /// <summary>
    /// 字符串 Base64 加密
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="encoding">为空则默认编码是 UTF-8</param>
    /// <returns></returns>
    public static string ToBase64(this string str, Encoding encoding = null)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }
        return Convert.ToBase64String((encoding ?? DefaultEncoding).GetBytes(str));
    }
    #endregion

    #region [FromBase64]
    /// <summary>
    /// 字符串 Base64 解密
    /// </summary>
    /// <param name="str">Base64 字符串</param>
    /// <param name="encoding">为空则默认编码是 UTF-8</param>
    /// <returns></returns>
    public static string FromBase64(this string str, Encoding encoding = null)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }
        return (encoding ?? DefaultEncoding).GetString(Convert.FromBase64String(str));
    }
    #endregion

    #region [ToBytes]
    /// <summary>
    /// 字符串转换到字节
    /// </summary>
    /// <param name="str"></param>
    /// <param name="encoding">为空则默认编码是 UTF-8</param>
    /// <returns></returns>
    public static byte[] ToBytes(this string str, Encoding encoding = null)
    {
        if (string.IsNullOrEmpty(str))
        {
            return new byte[] { };
        }
        return (encoding ?? DefaultEncoding).GetBytes(str);
    }
    #endregion

    #region [ToFile]
    /// <summary>
    /// 字符串写入到文件
    /// </summary>
    /// <param name="str"></param>
    /// <param name="fileName"></param>
    /// <param name="encoding"></param>
    public static void ToFile(this string str, string fileName, Encoding encoding = null)
    {
        var bytes = str.ToBytes(encoding);
        bytes.ToFile(fileName);
    }
    #endregion

    #region [AppendToFile]
    /// <summary>
    /// 字符串追加到文件
    /// </summary>
    /// <param name="str"></param>
    /// <param name="fileName"></param>
    /// <param name="encoding"></param>
    public static void AppendToFile(this string str, string fileName, Encoding encoding = null)
    {
        var bytes = str.ToBytes(encoding);
        bytes.AppendToFile(fileName);
    }
    #endregion

    #region [CopyFileTo]
    /// <summary>
    /// 文件复制（注意：默认 overwrite 为 true，目标文件存在会被覆盖）
    /// </summary>
    /// <param name="fromFileName">源文件</param>
    /// <param name="toFileName">目标文件</param>
    /// <param name="overwrite">是否覆盖，默认为 true</param>
    public static void CopyFileTo(this string fromFileName, string toFileName, bool overwrite = true)
    {
        File.Copy(fromFileName, toFileName, true);
    }
    #endregion

    #region [MoveFileTo]
    /// <summary>
    /// 文件移动
    /// </summary>
    /// <param name="fromFileName"></param>
    /// <param name="toFileName"></param>
    public static void MoveFileTo(this string fromFileName, string toFileName)
    {
        File.Move(fromFileName, toFileName);
    }
    #endregion

    #region [ToStream]
    /// <summary>
    /// 将一个文件读取到 MemoryStream
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static MemoryStream ToStream(this string fileName)
    {
        return new MemoryStream(File.ReadAllBytes(fileName));
    }
    #endregion

    #region [ToHex/HexStringToHexBytes/HexStringToString]
    /// <summary>
    /// 字符串转化成16进制字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static string ToHex(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }
        var bytes = str.ToBytes();
        var sb = new StringBuilder();
        foreach (var item in bytes)
        {
            sb.Append(item.ToString("x2"));
        }
        return sb.ToString();
    }
    /// <summary>
    /// 16进制的字符串转化到16进制的字节数组
    /// </summary>
    /// <param name="hexString">16进制的字符串</param>
    /// <returns></returns>
    public static byte[] HexStringToHexBytes(this string hexString)
    {
        hexString = hexString.Replace(" ", "");
        if ((hexString.Length % 2) != 0)
            hexString += " ";
        byte[] returnBytes = new byte[hexString.Length / 2];
        for (int i = 0; i < returnBytes.Length; i++)
            returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        return returnBytes;
    }

    /// <summary>
    /// 将一个 16 进制的字符串解码为字符串
    /// </summary>
    /// <param name="hexString"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string HexStringToString(this string hexString, Encoding encoding = null)
    {
        return (encoding ?? DefaultEncoding).GetString(hexString.HexStringToHexBytes());
    }
    #endregion

    #region [DESEncrypt]
    /// <summary>
    /// DES 加密
    /// </summary>
    /// <param name="inputString">字符串</param>
    /// <param name="key">密码</param>
    /// <param name="iv">加密向量</param>
    /// <returns></returns>
    public static string DESEncrypt(this string inputString, byte[] key, byte[] iv)
    {
        return new MrHuo.Extensions.DES().Encrypt(inputString, key, iv);
    }

    /// <summary>
    /// DES 加密，密码为字符串
    /// </summary>
    /// <param name="inputString">字符串</param>
    /// <param name="key">密码</param>
    /// <param name="iv">加密向量</param>
    /// <returns></returns>
    public static string DESEncrypt(this string inputString, string key, byte[] iv)
    {
        return new MrHuo.Extensions.DES().Encrypt(inputString, key, iv);
    }

    /// <summary>
    /// DES 加密，密码为字符串，加密向量默认为本类的全名
    /// </summary>
    /// <param name="inputString">字符串</param>
    /// <param name="key">密码</param>
    /// <returns></returns>
    public static string DESEncrypt(this string inputString, string key)
    {
        return new MrHuo.Extensions.DES().Encrypt(inputString, key);
    }
    #endregion

    #region [DESDecrypt]
    /// <summary>
    /// DES 解密
    /// </summary>
    /// <param name="inputString">字符串</param>
    /// <param name="key">密码</param>
    /// <param name="iv">加密向量</param>
    /// <returns></returns>
    public static string DESDecrypt(this string inputString, byte[] key, byte[] iv)
    {
        return new MrHuo.Extensions.DES().Decrypt(inputString, key, iv);
    }

    /// <summary>
    /// DES 解密，密码为字符串
    /// </summary>
    /// <param name="inputString">字符串</param>
    /// <param name="key">密码</param>
    /// <param name="iv">加密向量</param>
    /// <returns></returns>
    public static string DESDecrypt(this string inputString, string key, byte[] iv)
    {
        return new MrHuo.Extensions.DES().Decrypt(inputString, key, iv);
    }

    /// <summary>
    /// DES 解密，密码为字符串，加密向量默认为本类的全名
    /// </summary>
    /// <param name="inputString">字符串</param>
    /// <param name="key">密码</param>
    /// <returns></returns>
    public static string DESDecrypt(this string inputString, string key)
    {
        return new MrHuo.Extensions.DES().Decrypt(inputString, key);
    }
    #endregion

    #region [Md5]
    /// <summary>
    /// MD5 加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="encoding">为空则默认编码是 UTF-8</param>
    /// <returns></returns>
    public static string ToMd5(this string str, Encoding encoding = null)
    {
        using (var md5 = MD5.Create())
        {
            var result = md5.ComputeHash((encoding ?? DefaultEncoding).GetBytes(str));
            var strResult = BitConverter.ToString(result);
            return strResult.Replace("-", "");
        }
    }
    #endregion

    #region [ToFileMd5]
    /// <summary>
    /// 获取文件的 MD5
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string ToFileMd5(this string fileName)
    {
        using (var fs = new FileStream(fileName, FileMode.Open))
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();
            return retVal.ToHex();
        }
    }
    #endregion

    #region [HtmlEncode/HtmlDecode]
    /// <summary>
    /// 将一个字符串HTML编码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string HtmlEncode(this string str)
    {
        return HttpUtility.HtmlEncode(str);
    }

    /// <summary>
    /// 将一个字符串HTML解码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string HtmlDecode(this string str)
    {
        return HttpUtility.HtmlDecode(str);
    }
    #endregion

    #region [UrlEncode/UrlDecode]
    /// <summary>
    /// 将一个字符串URL编码
    /// </summary>
    /// <param name="value"></param>
    /// <param name="encoding">为空则默认是 UTF-8</param>
    /// <returns></returns>
    public static string UrlEncode(this string value, Encoding encoding = null)
    {
        return HttpUtility.UrlEncode(value, encoding ?? DefaultEncoding);
    }
    /// <summary>
    /// 将一个字符串URL解码
    /// </summary>
    /// <param name="value"></param>
    /// <param name="encoding">为空则默认是 UTF-8</param>
    /// <returns></returns>
    public static string UrlDecode(this string value, Encoding encoding = null)
    {
        return HttpUtility.UrlDecode(value, encoding ?? DefaultEncoding);
    }
    #endregion

    #region [ParseQueryString]
    /// <summary>
    /// 将一个 QueryString 字符串转化成字典
    /// </summary>
    /// <param name="query"></param>
    /// <param name="encoding">为空则默认编码是 UTF-8</param>
    /// <returns></returns>
    public static Dictionary<string, string> ParseQueryString(this string query, Encoding encoding = null)
    {
        var nv = HttpUtility.ParseQueryString(query, encoding ?? DefaultEncoding);
        return nv.ToDictionary();
    }
    #endregion

    #region [Join]
    /// <summary>
    /// 将一个字符串数组 JOIN 成一个字符串，默认分隔符为 string.Empty
    /// </summary>
    /// <param name="stringArray"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string Join(this string[] stringArray, string separator = "")
    {
        return string.Join(separator, stringArray);
    }
    #endregion

    #region [CleanHtml]
    /// <summary>
    /// 清除 HTML 标记
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string CleanHtml(this string html)
    {
        string strText = Regex.Replace(html, "<[^>]+>", "");
        strText = Regex.Replace(strText, "&[^;]+;", "");
        return strText;
    }
    #endregion

    #region [SubStringEx]
    /// <summary>
    /// 扩展的 SubString，不会抛异常，如果超出范围，返回空字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string SubStringEx(this string str, int start, int length)
    {
        if (str.Length == 0 || start < 0 || length < 0 || start > str.Length - 1)
        {
            return string.Empty;
        }
        if (length > str.Length - 1)
        {
            length = str.Length - 1;
        }
        return str.Substring(start, length);
    }
    #endregion

    #region [HttpGet]
    /// <summary>
    /// GET 请求 URL
    /// </summary>
    /// <param name="url">URL地址，非URL地址抛出异常</param>
    /// <param name="throwException">是否抛出异常</param>
    /// <returns></returns>
    public static string HttpGet(this string url, bool throwException = false)
    {
        var response = HttpClientHelper.Get(url);
        if (response.Error != null && throwException)
        {
            throw response.Error;
        }
        return response.Data;
    }
    /// <summary>
    /// GET 请求 URL，反序列化为对象
    /// </summary>
    /// <param name="url">URL地址，非URL地址抛出异常</param>
    /// <param name="throwException">是否抛出异常</param>
    /// <returns></returns>
    public static T HttpGet<T>(this string url, bool throwException = false)
    {
        var response = HttpClientHelper.Get<T>(url);
        if (response.Error != null && throwException)
        {
            throw response.Error;
        }
        return response.Data;
    }
    #endregion

    #region [HttpPost]
    /// <summary>
    /// POST 请求 URL
    /// </summary>
    /// <param name="url">URL地址，非URL地址抛出异常</param>
    /// <param name="data">POST 数据</param>
    /// <param name="throwException">是否抛出异常</param>
    /// <returns></returns>
    public static string HttpPost(this string url, Dictionary<string, object> data = null, bool throwException = false)
    {
        var response = HttpClientHelper.Post(url, data);
        if (response.Error != null && throwException)
        {
            throw response.Error;
        }
        return response.Data;
    }
    /// <summary>
    /// POST 请求 URL，反序列化为对象
    /// </summary>
    /// <param name="url">URL地址，非URL地址抛出异常</param>
    /// <param name="data">POST 数据</param>
    /// <param name="throwException">是否抛出异常</param>
    /// <returns></returns>
    public static T HttpPost<T>(this string url, Dictionary<string, object> data = null, bool throwException = false)
    {
        var response = HttpClientHelper.Post<T>(url, data);
        if (response.Error != null && throwException)
        {
            throw response.Error;
        }
        return response.Data;
    }
    #endregion
}