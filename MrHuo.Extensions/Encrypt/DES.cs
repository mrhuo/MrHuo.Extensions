using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MrHuo.Extensions
{
    /// <summary>
    /// DES 加密/解密
    /// </summary>
    public class DES
    {
        public static byte[] DefaultIV = Encoding.UTF8.GetBytes($"MrHuo.Extensions");
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <param name="key">密码</param>
        /// <param name="iv">加密向量</param>
        /// <returns></returns>
        public string Encrypt(string inputString, byte[] key, byte[] iv)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    var des = new DESCryptoServiceProvider();
                    using (var cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(inputString);
                            sw.Flush();
                            cs.FlushFinalBlock();
                            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// DES 加密，密码为字符串
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <param name="key">密码</param>
        /// <param name="iv">加密向量</param>
        /// <returns></returns>
        public string Encrypt(string inputString, string key, byte[] iv)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new Exception("需要DES加密的字符串不能为空！");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("DES加密时密码不能为空！");
            }
            var keys = key.PadLeft(8, '0').ToBytes().Take(8).ToArray();
            return Encrypt(inputString, keys, iv);
        }

        /// <summary>
        /// DES 加密，密码为字符串，加密向量默认为本类的哈希值
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public string Encrypt(string inputString, string key)
        {
            return Encrypt(inputString, key, DefaultIV);
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public string Decrypt(string inputString, byte[] key, byte[] iv)
        {
            try
            {
                if (string.IsNullOrEmpty(inputString))
                {
                    throw new Exception("需要DES解密的字符串不能为空！");
                }
                using (var ms = new MemoryStream(Convert.FromBase64String(inputString)))
                {
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    using (var cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// DES 解密，密码为字符串
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <param name="key">密码</param>
        /// <param name="iv">加密向量</param>
        /// <returns></returns>
        public string Decrypt(string inputString, string key, byte[] iv)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("DES解密时密码不能为空！");
            }
            var keys = key.PadLeft(8, '0').ToBytes().Take(8).ToArray();
            return Decrypt(inputString, keys, iv);
        }

        /// <summary>
        /// DES解密。
        /// </summary>
        /// <param name="inputString">已加密字符串</param>
        /// <param name="key">密码</param>
        /// <returns>解密后的字符串。</returns>
        public string Decrypt(string inputString, string key)
        {
            return Decrypt(inputString, key, DefaultIV);
        }
    }
}
