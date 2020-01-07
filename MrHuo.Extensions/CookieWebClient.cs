using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MrHuo.Extensions
{
    /// <summary>
    /// 带 Cookies 请求的 WebClient
    /// </summary>
    public class CookieWebClient : WebClient
    {
        private CookieContainer cookieContainer = new CookieContainer();

        /// <summary>
        /// 获取最后访问的 URL
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 清空 Cookies
        /// </summary>
        public CookieWebClient ClearCookies()
        {
            cookieContainer = new CookieContainer();
            return this;
        }

        /// <summary>
        /// 手动添加 Cookies
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public CookieWebClient AddCookies(Dictionary<string, string> cookies)
        {
            foreach (var item in cookies)
            {
                cookieContainer.Add(new Cookie(item.Key, item.Value));
            }
            return this;
        }

        /// <summary>
        /// 手动添加 Cookies
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public CookieWebClient AddCookies(List<Cookie> cookies)
        {
            foreach (var item in cookies)
            {
                cookieContainer.Add(item);
            }
            return this;
        }

        /// <summary>
        /// 重写请求，加入 Cookies
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(System.Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                var httpWebRequest = (HttpWebRequest)request;
                httpWebRequest.CookieContainer = cookieContainer;
                if (Referer != null)
                {
                    httpWebRequest.Referer = Referer;
                }
            }
            Referer = address.ToString();
            return request;
        }
    }
}
