using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace MrHuo.Extensions
{
    /// <summary>
    /// 请求API
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        /// GET 请求，返回网页内容
        /// </summary>
        /// <param name="api"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static (string Data, Exception Error) Get(string api, Dictionary<string, object> headers = null)
        {
            using (var client = new WebClient())
            {
                try
                {
                    if (headers!=null)
                    {
                        foreach (var item in headers)
                        {
                            client.Headers.Add(item.Key, $"{item.Value}");
                        }
                    }
                    return (client.DownloadData(api).ToStringEx(), null);
                }
                catch (Exception ex)
                {
                    return (null, ex);
                }
            }
        }

        /// <summary>
        /// GET 请求（反序列化成对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static (T Data, Exception Error) Get<T>(string api, Dictionary<string, object> headers = null)
        {
            using (var client = new WebClient())
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
                    return (client.DownloadData(api).ToStringEx().FromJson<T>(), null);
                }
                catch (Exception ex)
                {
                    return (default(T), ex);
                }
            }
        }

        /// <summary>
        /// POST 请求，返回网页内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static (string Data, Exception Error) Post(string api, Dictionary<string, object> data = null, Dictionary<string, object> headers = null)
        {
            data = data ?? new Dictionary<string, object>();
            using (var client = new WebClient())
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
                    return (client.UploadValues(api, data.ToNameValueCollection()).ToStringEx(), null);
                }
                catch (Exception ex)
                {
                    return (null, ex);
                }
            }
        }

        /// <summary>
        /// POST 请求（反序列化成对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static (T Data, Exception Error) Post<T>(string api, Dictionary<string, object> data = null, Dictionary<string, object> headers = null)
        {
            data = data ?? new Dictionary<string, object>();
            using (var client = new WebClient())
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
                    return (client.UploadValues(api, data.ToNameValueCollection()).ToStringEx().FromJson<T>(), null);
                }
                catch (Exception ex)
                {
                    return (default(T), ex);
                }
            }
        }
    }
}
