using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.IO.Compression;

namespace HHTravel.DataService
{
    class CookieTempOrderProvider : ITempOrderProvider
    {
        public void AddOrUpdate(Guid sessionId, string content)
        {
            SetCookie("TempOrder" + sessionId, content);
        }

        public string GetContent(Guid sessionId)
        {
            string content = GetCookie("TempOrder" + sessionId);
            return content;
        }


        #region Cookie操作

        /// <summary>
        /// 清除所有
        /// </summary>
        public static void Clear()
        {
            foreach (HttpCookie c in HttpContext.Current.Response.Cookies)
            {
                Delete(c);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="name"></param>
        private static void Delete(HttpCookie cookie)
        {
            if (cookie == null)
            {
                return;
            }
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetCookie(string name)
        {
            string value = null;

            var cookie = HttpContext.Current.Request.Cookies[name];

            if (cookie != null)
            {
                var bytes = Convert.FromBase64String(cookie.Value);
                value = UnZipStr(bytes);
            }
            return value;
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void SetCookie(string name, string value)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];

            value = Convert.ToBase64String(ZipStr(value));

            if (cookie == null)
            {
                cookie = new HttpCookie(name);
                cookie.Domain = "";
                cookie.Path = "/";
                cookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Value = value;
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
        #endregion Cookie操作


        public static string UnZipStr(byte[] input)
        {
            using (MemoryStream inputStream = new MemoryStream(input))
            {
                using (DeflateStream gzip =
                  new DeflateStream(inputStream, CompressionMode.Decompress))
                {
                    using (StreamReader reader =
                      new StreamReader(gzip, System.Text.Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public static byte[] ZipStr(String str)
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (DeflateStream gzip =
                  new DeflateStream(output, CompressionMode.Compress))
                {
                    using (StreamWriter writer =
                      new StreamWriter(gzip, System.Text.Encoding.UTF8))
                    {
                        writer.Write(str);
                    }
                }

                return output.ToArray();
            }
        }
    }
}
