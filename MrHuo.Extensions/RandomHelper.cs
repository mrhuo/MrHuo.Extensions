using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrHuo.Extensions
{
    /// <summary>
    /// 随机字符串帮助类
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// 26个小写字母
        /// </summary>
        public static readonly string LowercaseString = "abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// 26个大写字母
        /// </summary>
        public static readonly string UppercaseString = LowercaseString.ToUpper();
        /// <summary>
        /// 10个数字
        /// </summary>
        public static readonly string NumberString = "01234567890";

        /// <summary>
        /// 获取随机生成器
        /// </summary>
        /// <returns></returns>
        private static Random GetRandom()
        {
            return new Random(Guid.NewGuid().GetHashCode());
        }

        /// <summary>
        /// 生成指定长度的随机数字字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomNumberString(int length)
        {
            return RandomString(length, false, false, true);
        }

        /// <summary>
        /// 随机 bool 
        /// </summary>
        /// <returns></returns>
        public static bool RandomBool()
        {
            return GetRandom().NextDouble() > 0.5;
        }

        /// <summary>
        /// 生成指定范围内的随机数字
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int RandomNumber(int min, int max)
        {
            return GetRandom().Next(min, max);
        }

        /// <summary>
        /// 生成随机字符串，包含：小写字母，大写字母，数字，可输入用户字典扩展
        /// <para>默认生成10个随机字符</para>
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="includeLower">是否包含小写字母</param>
        /// <param name="includeUpper">是否包含大写字母</param>
        /// <param name="includeNumber"></param>
        /// <param name="userDict">用户提供的字典</param>
        /// <returns></returns>
        public static string RandomString(
            int length = 10, 
            bool includeLower = true, 
            bool includeUpper = true, 
            bool includeNumber = true, 
            string userDict = "")
        {
            var str = 
                $"{(includeLower ? LowercaseString : "")}" +
                $"{(includeUpper ? UppercaseString : "")}" +
                $"{(includeNumber ? NumberString : "")}" +
                $"{userDict}";
            var ret = string.Empty;
            for (int i = 0; i < length; i++)
            {
                ret += str.Random(1).ToString(string.Empty);
            }
            return ret;
        }

        /// <summary>
        /// 从一个列表中随机获取一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T RandomOne<T>(IEnumerable<T> data)
        {
            if (data == null || !data.Any())
            {
                return default(T);
            }
            return data.OrderBy(p => GetRandom().Next()).FirstOrDefault();
        }

        /// <summary>
        /// 从一个列表中随机选择某几个元素，如果 take = 0，则返回随机排序的所有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static IEnumerable<T> RandomSome<T>(IEnumerable<T> data, int take = 0)
        {
            if (data == null || !data.Any())
            {
                return data;
            }
            var ret = data.OrderBy(p => GetRandom().Next());
            if (take>0)
            {
                return ret.Take(take);
            }
            return ret;
        }
    }
}
