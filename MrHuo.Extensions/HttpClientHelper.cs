using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace MrHuo.Extensions
{
    /// <summary>
    /// HttpClient 帮助类
    /// </summary>
    public static class HttpClientHelper
    {
        public static Encoding DefaultEncoding = Encoding.UTF8;

        #region [GET]
        /// <summary>
        /// Get 请求，返回字节码
        /// </summary>
        /// <param name="api"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static (byte[] Data, Exception Error) GetBytes(string api, Dictionary<string, object> headers = null)
        {
            using (var client = new WebClient() { Encoding = DefaultEncoding })
            {
                try
                {
                    if (headers != null)
                    {
                        foreach (var item in headers)
                        {
                            client.Headers.Add(item.Key, $"{item.Value}");
                        }
                    }
                    return (client.DownloadData(api), null);
                }
                catch (Exception ex)
                {
                    return (null, ex);
                }
            }
        }

        /// <summary>
        /// GET 请求，返回网页内容
        /// </summary>
        /// <param name="api"></param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static (string Data, Exception Error) Get(string api, Dictionary<string, object> headers = null, Encoding encoding = null)
        {
            var (Data, Error) = GetBytes(api, headers);
            if (Error != null)
            {
                return (null, Error);
            }
            return (Data.ToStringEx(encoding), null);
        }

        /// <summary>
        /// GET 请求（反序列化成对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static (T Data, Exception Error) Get<T>(string api, Dictionary<string, object> headers = null, Encoding encoding = null)
        {
            var (Data, Error) = Get(api, headers, encoding);
            if (Error != null)
            {
                return (default(T), Error);
            }
            return (Data.FromJson<T>(), null);
        }
        #endregion

        #region [POST]
        /// <summary>
        /// POST 请求，返回网页内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static (byte[] Data, Exception Error) PostBytes(string api, Dictionary<string, object> data = null, Dictionary<string, object> headers = null)
        {
            data = data ?? new Dictionary<string, object>();
            using (var client = new WebClient() { Encoding = DefaultEncoding })
            {
                try
                {
                    if (headers != null)
                    {
                        foreach (var item in headers)
                        {
                            client.Headers.Add(item.Key, $"{item.Value}");
                        }
                    }
                    return (client.UploadValues(api, data.ToNameValueCollection()), null);
                }
                catch (Exception ex)
                {
                    return (null, ex);
                }
            }
        }

        /// <summary>
        /// POST 请求，返回网页内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static (string Data, Exception Error) Post(string api, Dictionary<string, object> data = null, Dictionary<string, object> headers = null, Encoding encoding = null)
        {
            var (Data, Error) = PostBytes(api, data, headers);
            if (Error != null)
            {
                return (null, Error);
            }
            return (Data.ToStringEx(encoding), null);
        }

        /// <summary>
        /// POST 请求（反序列化成对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static (T Data, Exception Error) Post<T>(string api, Dictionary<string, object> data = null, Dictionary<string, object> headers = null, Encoding encoding = null)
        {
            var (Data, Error) = Post(api, data, headers, encoding);
            if (Error != null)
            {
                return (default(T), Error);
            }
            return (Data.FromJson<T>(), null);
        }
        #endregion
    }
}
