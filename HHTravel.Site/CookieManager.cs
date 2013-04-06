using System;
using System.Web;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Site
{
    /// <summary>
    /// CookieManager
    /// </summary>
    public class CookieManager
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public static string Captcha
        {
            get { return GetCookie("Captcha"); }
            set { SetCookie("Captcha", value); }
        }

        /// <summary>
        /// 出发地
        /// </summary>
        public static DepartureCity? DepartureCity
        {
            get { return Infrastructure.Crosscutting.DepartureCity.Parse(GetCookie("DepartureCityCode")); }
            set { SetCookie("DepartureCityCode", value != null ? value.Value.CityCode : null); }
        }

        /// <summary>
        /// 是否跳转到台北网站
        /// </summary>
        [Obsolete("判断结果暂不记入Cookie 2012-12-05", true)]
        public static bool? IsRedirectTB
        {
            get
            {
                bool r;
                string cookie = GetCookie("IsRedirectTB");
                return bool.TryParse(cookie, out r) ? r : (bool?)null;
            }
            set { SetCookie("IsRedirectTB", value.ToString()); }
        }

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
            var cookie = HttpContext.Current.Request.Cookies[name];
            return (cookie != null) ? HttpUtility.UrlDecode(cookie.Value) : null;
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void SetCookie(string name, string value)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];

            if (!string.IsNullOrEmpty(value))
            {
                if (cookie == null)
                {
                    cookie = new HttpCookie(name);
                }
                cookie.Value = HttpUtility.UrlEncode(value);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                Delete(cookie);
            }
        }
    }
}