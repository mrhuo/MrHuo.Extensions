using NUnit.Framework;
using System;

namespace Tests
{
    public class StringExtensionsTest
    {
        [Test]
        public void TestCount()
        {
            var str = "CABCDabcdecdCfcgdcd";
            Assert.AreEqual(0, "".Count("a"));
            Assert.AreEqual(1, str.Count(str));
            Assert.AreEqual(3, str.Count("cd"));
            Assert.AreEqual(4, str.Count("CD", true));
            Assert.AreEqual(4, str.Count('c'));
            Assert.AreEqual(7, str.Count('C', true));
        }

        [Test]
        public void TestRepeat()
        {
            var str = "hai";
            Assert.AreEqual("haihaihai", str.Repeat(3));
            Assert.AreEqual("hai", str.Repeat(0));
            Assert.AreEqual(null, ((string)null)?.Repeat(0));
        }

        [Test]
        public void TestCapitalize()
        {
            var str = "abcdefg";
            Assert.AreEqual("Abcdefg", str.Capitalize());
            Assert.AreEqual("ABCdefg", str.Capitalize(3));
            Assert.AreEqual("abCDEfg", str.Capitalize(2, 3));
        }

        [Test]
        public void TestIsUpper()
        {
            Assert.True("ABC".IsUpper());
            Assert.False("ABc".IsUpper());
        }

        [Test]
        public void TestIsLower()
        {
            Assert.False("ABC".IsLower());
            Assert.True("abc".IsLower());
        }

        [Test]
        public void TestCenter()
        {
            var str = "hello";
            Assert.AreEqual("    hello    ", str.PadCenter(4 * 2 + str.Length));
            Assert.AreEqual("====hello====", str.PadCenter('=', 4 * 2 + str.Length));
        }

        [Test]
        public void TestBase64()
        {
            var str = "hello";
            Assert.AreEqual("aGVsbG8=", str.ToBase64());
        }

        [Test]
        public void TestReverse()
        {
            var str = "hello";
            Assert.AreEqual("olleh", str.Reverse());
            Assert.AreEqual("hello", str.Reverse().Reverse());
        }

        [Test]
        public void TestLeft()
        {
            var str = "hello";
            Assert.AreEqual(str, str.Left(-1));
            Assert.AreEqual("he", str.Left(2));
            Assert.AreEqual(str, str.Left(10));
        }

        [Test]
        public void TestRight()
        {
            var str = "hello";
            Assert.AreEqual(str, str.Right(-1));
            Assert.AreEqual("lo", str.Right(2));
            Assert.AreEqual(str, str.Right(10));
        }

        [Test]
        public void TestSplitLines()
        {
            var str = "aa\nbb\r\ncc";
            var expert = new string[] { "aa","bb","cc" };
            Assert.AreEqual(expert, str.SplitLines());
        }

        [Test]
        public void TestRemove()
        {
            var str = "aabbccdd";
            Assert.AreEqual("aabbdd", str.Remove("cc"));
            Assert.AreEqual("aabbdd", str.Remove("c"));
        }

        [Test]
        public void TestTo()
        {
            var str = "123";
            Assert.AreEqual(123, str.To<int>());
            Assert.AreEqual(123L, str.To<long>());
            Assert.AreEqual(123F, str.To<float>());
            Assert.AreEqual(123D, str.To<double>());
            Assert.AreEqual(true, "true".To<bool>());
            Assert.AreEqual(false, "123".To<bool>());
            Assert.AreEqual(false, "false".To<bool>());
        }

        [Test]
        public void TestToHex()
        {
            var str = "123abc";
            Assert.AreEqual("313233616263", str.ToHex());
        }

        [Test]
        public void TestDESEncryptAndDESDecrypt()
        {
            var str = "abc";
            Assert.AreEqual("qonwqDWxm9I=", str.DESEncrypt("123456"));
            Assert.AreEqual(str, "qonwqDWxm9I=".DESDecrypt("123456"));
        }

        [Test]
        public void TestToMd5()
        {
            Assert.AreEqual("E10ADC3949BA59ABBE56E057F20F883E", "123456".ToMd5());
            Assert.AreEqual("900150983CD24FB0D6963F7D28E17F72", "abc".ToMd5());
        }

        [Test]
        public void TestHtmlEncodeAndHtmlDecode()
        {
            var str = "<h1>test</h1>";
            Assert.AreEqual("&lt;h1&gt;test&lt;/h1&gt;", str.HtmlEncode());
            Assert.AreEqual(str, "&lt;h1&gt;test&lt;/h1&gt;".HtmlDecode());
        }

        [Test]
        public void TestUrlEncodeAndUrlDecode()
        {
            var str = "?name=abc";
            Assert.AreEqual("%3fname%3dabc", str.UrlEncode());
            Assert.AreEqual(str, "%3fname%3dabc".UrlDecode());
        }

    }
}